using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Handlers;

public interface IPermissionCommandHandler
{
    Task<Result<PermissionDto>> CreatePermission(CreatePermissionDto command, CancellationToken cancellationToken);
    Task<Result<PermissionDto>> UpdatePermission(Guid id, UpdatePermissionDto command, CancellationToken cancellationToken);
    Task<Result> DeletePermission(Guid id, CancellationToken cancellationToken);
}
