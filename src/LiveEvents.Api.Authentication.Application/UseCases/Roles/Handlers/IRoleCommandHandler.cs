using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Handlers;

public interface IRoleCommandHandler
{
    Task<Result<RoleDto>> CreateRole(CreateRoleDto command, CancellationToken cancellationToken);
    Task<Result<RoleDto>> UpdateRole(Guid id, UpdateRoleDto command, CancellationToken cancellationToken);
    Task<Result> DeleteRole(Guid id, CancellationToken cancellationToken);

    // Permission management methods
    Task<Result<MultiplePermissionRemovalResult>> RemoveMultiplePermissionsFromRole(RemoveMultiplePermissionsFromRole command, CancellationToken cancellationToken);
}
