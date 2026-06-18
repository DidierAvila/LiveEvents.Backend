using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Handlers;

public class PermissionCommandHandler : IPermissionCommandHandler
{
    private readonly CreatePermission _createPermission;
    private readonly UpdatePermission _updatePermission;
    private readonly DeletePermission _deletePermission;

    public PermissionCommandHandler(
        CreatePermission createPermission,
        UpdatePermission updatePermission,
        DeletePermission deletePermission)
    {
        _createPermission = createPermission;
        _updatePermission = updatePermission;
        _deletePermission = deletePermission;
    }

    public async Task<Result<PermissionDto>> CreatePermission(CreatePermissionDto command, CancellationToken cancellationToken)
    {
        return await _createPermission.HandleAsync(command, cancellationToken);
    }

    public async Task<Result<PermissionDto>> UpdatePermission(Guid id, UpdatePermissionDto command, CancellationToken cancellationToken)
    {
        return await _updatePermission.HandleAsync(id, command, cancellationToken);
    }

    public async Task<Result> DeletePermission(Guid id, CancellationToken cancellationToken)
    {
        return await _deletePermission.HandleAsync(id, cancellationToken);
    }
}
