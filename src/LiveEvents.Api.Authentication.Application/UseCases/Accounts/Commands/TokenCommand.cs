using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using LiveEvents.Api.Authentication.Application.Security;
using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Commands;

public class TokenCommand
{
    private readonly IRepositoryBase<Session> _sessionRepository;
    private readonly ILogger<TokenCommand> _logger;
    private readonly IConfiguration _configuration;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly ISecurityStampService _securityStampService;

    public TokenCommand(
        IRepositoryBase<Session> sessionRepository,
        IConfiguration configuration,
        IRolePermissionRepository rolePermissionRepository,
        IRepositoryBase<Permission> permissionRepository,
        IUserRoleRepository userRoleRepository,
        ISecurityStampService securityStampService,
        ILogger<TokenCommand> logger)
    {
        _sessionRepository = sessionRepository;
        _logger = logger;
        _configuration = configuration;
        _rolePermissionRepository = rolePermissionRepository;
        _permissionRepository = permissionRepository;
        _userRoleRepository = userRoleRepository;
        _securityStampService = securityStampService;
    }

    public async Task<string> HandleAsync(User user, CancellationToken cancellationToken)
    {
        var nowUtc = DateTime.UtcNow;
        var nowDb = DateTime.SpecifyKind(nowUtc, DateTimeKind.Unspecified);

        var currentSession = await _sessionRepository.Find(x => x.UserId == user.Id && x.Expires > nowDb, cancellationToken);
        if (currentSession != null)
        {
            if (currentSession.Expires.CompareTo(nowDb) < 0)
            {
                _logger.LogInformation("GetToken: Expiration Session UserId:" + user.Id);
                return await RefreshSessionAsync(currentSession, user, cancellationToken);
            }

            var currentSecurityStamp = await _securityStampService.EnsureSecurityStampAsync(user, cancellationToken);
            var tokenSecurityStamp = TryGetTokenSecurityStamp(currentSession.SessionToken);

            if (string.IsNullOrWhiteSpace(tokenSecurityStamp) ||
                !string.Equals(currentSecurityStamp, tokenSecurityStamp, StringComparison.Ordinal))
            {
                return await RefreshSessionAsync(currentSession, user, cancellationToken);
            }

            return currentSession.SessionToken!;
        }

        var token = await GenerateTokenAsync(user, cancellationToken);
        return token ?? string.Empty;
    }

    private async Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken)
    {
        string? key = _configuration.GetValue<string>("JwtSettings:key");
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // Obtener permisos del usuario
        List<string> userPermissions = await GetUserPermissionsAsync(user.Id, cancellationToken);

        var securityStamp = await _securityStampService.EnsureSecurityStampAsync(user, cancellationToken);

        // Crear los claims básicos
        var claims = new List<Claim>
        {
            new(CustomClaimTypes.UserId, user.Id.ToString()),
            new(CustomClaimTypes.UserName, user.Name),
            new(CustomClaimTypes.UserEmail, user.Email),
            new(CustomClaimTypes.UserTypeId, user.UserTypeId.ToString()),
            new(CustomClaimTypes.UserTypeName, user.UserType!.Name!),
            new(CustomClaimTypes.SecurityStamp, securityStamp),
        };

        // Agregar permisos como claims
        foreach (var permission in userPermissions)
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, permission));
        }

        // Crear el token
        var expiresUtc = DateTime.UtcNow.AddMinutes(60);
        var expiresDb = DateTime.SpecifyKind(expiresUtc, DateTimeKind.Unspecified);
        var issuer = _configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = _configuration.GetValue<string>("JwtSettings:Audience");
        JwtSecurityToken tokenJwt = new(
            issuer,
            audience,
            claims,
            expires: expiresUtc,
            signingCredentials: credentials
        );

        string Newtoken = new JwtSecurityTokenHandler().WriteToken(tokenJwt);

        //Se almacena el nuevo token como session
        if (!string.IsNullOrEmpty(Newtoken))
        {
            Session session = new()
            {
                Id = Guid.NewGuid(),
                SessionToken = Newtoken,
                UserId = user.Id,
                Expires = expiresDb
            };

            await _sessionRepository.Create(session, cancellationToken);
        }
        return Newtoken;
    }

    private async Task<string> RefreshSessionAsync(Session session, User user, CancellationToken cancellationToken)
    {
        // Eliminar sesión anterior
        await _sessionRepository.Delete(session.Id, cancellationToken);

        string currentToken = await GenerateTokenAsync(user, cancellationToken);
        return currentToken;
    }

    private static string? TryGetTokenSecurityStamp(string? jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return null;
        }

        try
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            return token.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.SecurityStamp)?.Value;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Obtiene todos los permisos del usuario basado en sus roles
    /// </summary>
    private async Task<List<string>> GetUserPermissionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var permissions = new HashSet<string>();

        // Obtener roles del usuario
        var userRoles = await _userRoleRepository.GetUserRolesAsync(userId, cancellationToken);

        // Obtener permisos de cada rol
        foreach (var userRole in userRoles)
        {
            var rolePermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(userRole.RoleId, cancellationToken);

            foreach (var rolePermission in rolePermissions)
            {
                var permission = await _permissionRepository.Find(p => p.Id == rolePermission.PermissionId, cancellationToken);
                if (permission != null && permission.Status)
                {
                    permissions.Add(permission.Name);
                }
            }
        }

        return [.. permissions];
    }
}
