using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Authentication.Application.Security;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;

public class RemovePermissionFromRole
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly ISecurityStampService _securityStampService;

    public RemovePermissionFromRole(
        IRolePermissionRepository rolePermissionRepository,
        ISecurityStampService securityStampService)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _securityStampService = securityStampService;
    }

    public async Task<Result> HandleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var rolePermission = await _rolePermissionRepository.GetByRoleAndPermissionAsync(roleId, permissionId, cancellationToken);
        if (rolePermission == null)
        {
            return Result.Failure(Error.NotFound("RolePermission.NotFound", "No se encontró la asignación del permiso al rol especificado"));
        }

        await _rolePermissionRepository.RemovePermissionFromRoleAsync(roleId, permissionId, cancellationToken);
        await _securityStampService.RefreshSecurityStampByRoleAsync(roleId, cancellationToken);
        return Result.Success();
    }
}
