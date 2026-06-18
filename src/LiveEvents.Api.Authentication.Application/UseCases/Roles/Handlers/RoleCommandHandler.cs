using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Handlers;

public class RoleCommandHandler : IRoleCommandHandler
{
    private readonly CreateRole _createRole;
    private readonly UpdateRole _updateRole;
    private readonly DeleteRole _deleteRole;

    public RoleCommandHandler(CreateRole createRole, UpdateRole updateRole, DeleteRole deleteRole)
    {
        _createRole = createRole;
        _updateRole = updateRole;
        _deleteRole = deleteRole;
    }

    public async Task<Result<RoleDto>> CreateRole(CreateRoleDto command, CancellationToken cancellationToken)
    {
        return await _createRole.HandleAsync(command, cancellationToken);
    }

    public async Task<Result<RoleDto>> UpdateRole(Guid id, UpdateRoleDto command, CancellationToken cancellationToken)
    {
        return await _updateRole.HandleAsync(id, command, cancellationToken);
    }

    public async Task<Result> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        return await _deleteRole.HandleAsync(id, cancellationToken);
    }

    public Task<Result<MultiplePermissionRemovalResult>> RemoveMultiplePermissionsFromRole(RemoveMultiplePermissionsFromRole command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
