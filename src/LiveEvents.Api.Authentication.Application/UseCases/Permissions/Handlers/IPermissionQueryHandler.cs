using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Handlers;

public interface IPermissionQueryHandler
{
    Task<Result<PermissionDto>> GetPermissionById(Guid id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<PermissionDto>>> GetAllPermissions(CancellationToken cancellationToken);
    Task<Result<IEnumerable<PermissionDto>>> GetActivePermissions(CancellationToken cancellationToken);
    Task<Result<IEnumerable<PermissionSummaryDto>>> GetPermissionsSummary(CancellationToken cancellationToken);
    Task<Result<PaginationResponseDto<PermissionListResponseDto>>> GetAllPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken);
    Task<Result<IEnumerable<PermissionDropdownDto>>> GetPermissionsForDropdown(CancellationToken cancellationToken);
}