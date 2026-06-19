using AutoMapper;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Authentication.Application.Security;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;

public class AssignMultiplePermissionsToRole
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IRepositoryBase<Role> _roleRepository;
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<AssignMultiplePermissionsToRole> _logger;
    private readonly ISecurityStampService _securityStampService;
    private readonly IValidationService _validationService;

    public AssignMultiplePermissionsToRole(
        IRolePermissionRepository rolePermissionRepository,
        IRepositoryBase<Role> roleRepository,
        IRepositoryBase<Permission> permissionRepository,
        IMapper mapper,
        ISecurityStampService securityStampService,
        IValidationService validationService,
        ILogger<AssignMultiplePermissionsToRole> logger)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _mapper = mapper;
        _securityStampService = securityStampService;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<Result<MultiplePermissionAssignmentResult>> HandleAsync(AssignMultiplePermissionsToRoleDto request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validationService.ValidateAsync(
            request,
            "RolePermission.InvalidData",
            "Los datos para asignar permisos no son válidos.",
            cancellationToken);
        if (validationResult.IsFailure)
            return Result.Failure<MultiplePermissionAssignmentResult>(validationResult.Error);

        var result = new MultiplePermissionAssignmentResult
        {
            RoleId = request.RoleId
        };

        // Verificar que el rol existe
        var role = await _roleRepository.Find(r => r.Id == request.RoleId, cancellationToken);
        if (role == null)
        {
            return Result.Failure<MultiplePermissionAssignmentResult>(Error.NotFound("Role.NotFound", $"Role with ID {request.RoleId} not found"));
        }

        result.RoleName = role.Name;

        // Obtener los permisos existentes del rol para evitar duplicados
        var existingRolePermissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(request.RoleId, cancellationToken);
        var existingPermissionIds = existingRolePermissions?.Select(rp => rp.PermissionId).ToHashSet() ?? new HashSet<Guid>();

        // Procesar cada permiso individualmente
        foreach (var permissionId in request.PermissionIds)
        {
            try
            {
                // Verificar si ya existe la asignación
                if (existingPermissionIds.Contains(permissionId))
                {
                    var permission = await _permissionRepository.Find(p => p.Id == permissionId, cancellationToken);
                    result.ExistingPermissions.Add(permission?.Name ?? permissionId.ToString());
                    continue;
                }

                // Verificar que el permiso existe
                var permissionToAssign = await _permissionRepository.Find(p => p.Id == permissionId, cancellationToken);
                if (permissionToAssign == null)
                {
                    result.FailedAssignments.Add(new PermissionAssignmentError
                    {
                        PermissionId = permissionId,
                        PermissionName = permissionId.ToString(),
                        ErrorMessage = $"Permission with ID {permissionId} not found"
                    });
                    continue;
                }

                // Crear la asignación
                var rolePermission = new RolePermission
                {
                    RoleId = request.RoleId,
                    PermissionId = permissionId
                };

                var createdRolePermission = await _rolePermissionRepository.Create(rolePermission, cancellationToken);

                // Agregar al resultado exitoso
                result.SuccessfulAssignments.Add(new RolePermissionDto
                {
                    RoleId = request.RoleId,
                    PermissionId = permissionId,
                    RoleName = role.Name,
                    PermissionName = permissionToAssign.Name
                });
            }
            catch (Exception)
            {
                var permission = await _permissionRepository.Find(p => p.Id == permissionId, cancellationToken);
                result.FailedAssignments.Add(new PermissionAssignmentError
                {
                    PermissionId = permissionId,
                    PermissionName = permission?.Name ?? permissionId.ToString(),
                    ErrorMessage = "No fue posible asignar el permiso."
                });
            }
        }

        // Invalidar sesiones solo si se asignaron nuevos permisos exitosamente
        if (result.SuccessfulAssignments.Any())
        {
            try
            {
                var updatedUsers = await _securityStampService.RefreshSecurityStampByRoleAsync(request.RoleId, cancellationToken);

                _logger.LogInformation("Asignación múltiple de permisos completada para el rol {RoleId}. {SuccessfulCount} permisos asignados. SecurityStamp actualizado para {UpdatedUsers} usuarios",
                    request.RoleId, result.SuccessfulAssignments.Count, updatedUsers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar SecurityStamp después de asignar permisos al rol {RoleId}", request.RoleId);
            }
        }
        else
        {
            _logger.LogInformation("No se asignaron nuevos permisos al rol {RoleId}, omitiendo actualización de SecurityStamp", request.RoleId);
        }

        return Result.Success(result);
    }
}
