using Microsoft.Extensions.Logging;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using BC = BCrypt.Net.BCrypt;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Commands;

public class LoginCommand
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IRepositoryBase<UserType> _userTypeRepository;
    private readonly ITokenCommandHandler _tokenCommandHandler;
    private readonly ILogger<LoginCommand> _logger;

    public LoginCommand(
        IRepositoryBase<User> userRepository,
        ILogger<LoginCommand> logger,
        IRepositoryBase<UserType> userTypeRepository,
        ITokenCommandHandler tokenCommandHandler)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userTypeRepository = userTypeRepository;
        _tokenCommandHandler = tokenCommandHandler;
    }

    public async Task<Result<LoginResponseDto>> HandleAsync(LoginRequestDto autorizacion, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar usuario solo por email
            User? CurrentUser = await _userRepository.Find(x => x.Email == autorizacion.Email, cancellationToken);

            if (CurrentUser == null)
            {
                _logger.LogWarning("Login: user not found {Email}", autorizacion.Email);
                return Result.Failure<LoginResponseDto>(Error.Unauthorized("Login.InvalidCredentials", "Credenciales inválidas"));
            }

            if (CurrentUser.Status != UserStatus.Activo)
            {
                _logger.LogWarning("Login: inactive user {Email}", autorizacion.Email);
                return Result.Failure<LoginResponseDto>(Error.Forbidden("Login.InactiveUser", "Usuario inactivo. Contacte al administrador."));
            }

            // Verificar si el usuario existe y la contraseña es correcta
            if (!string.IsNullOrEmpty(CurrentUser.Password))
            {
                bool isPasswordValid = BC.Verify(autorizacion.Password, CurrentUser.Password);
                if (isPasswordValid)
                {
                    CurrentUser.UserType = await _userTypeRepository.Find(x => x.Id == CurrentUser.UserTypeId, cancellationToken);
                    _logger.LogInformation("Login: success for user {Email}", autorizacion.Email);
                    string CurrentToken = await _tokenCommandHandler.GetToken(CurrentUser, cancellationToken);
                    LoginResponseDto loginResponse = new()
                    { Token = CurrentToken };

                    return Result.Success(loginResponse);
                }
                else
                {
                    _logger.LogWarning("Login: invalid password for user {Email}", autorizacion.Email);
                    return Result.Failure<LoginResponseDto>(Error.Unauthorized("Login.InvalidCredentials", "Credenciales inválidas"));
                }
            }

            _logger.LogWarning("Login: user has no password {Email}", autorizacion.Email);
            return Result.Failure<LoginResponseDto>(Error.Unauthorized("Login.InvalidCredentials", "Credenciales inválidas"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login: unexpected error for user {Email}", autorizacion.Email);
            return Result.Failure<LoginResponseDto>(Error.Failure("Login.Error", "Error al intentar iniciar sesión"));
        }
    }
}
