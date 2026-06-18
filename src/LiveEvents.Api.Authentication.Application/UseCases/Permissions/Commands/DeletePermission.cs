using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Commands;

public class DeletePermission
{
    private readonly IRepositoryBase<Permission> _permissionRepository;

    public DeletePermission(IRepositoryBase<Permission> permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        // Find existing permission
        var permission = await _permissionRepository.Find(x => x.Id == id, cancellationToken);
        if (permission == null)
            return Result.Failure(Error.NotFound("Permission.NotFound", "Permission not found"));

        // Check if permission has associated roles
        if (permission.Roles?.Any() == true)
            return Result.Failure(Error.Conflict("Permission.HasRoles", "Cannot delete Permission with associated roles. Please remove the permission from all roles first."));

        // Delete permission
        await _permissionRepository.Delete(permission, cancellationToken);
        return Result.Success();
    }
}
