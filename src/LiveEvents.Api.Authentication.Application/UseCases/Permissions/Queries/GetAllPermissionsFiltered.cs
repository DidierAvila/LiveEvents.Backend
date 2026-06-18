using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;

public class GetAllPermissionsFiltered
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IPaginationServiceBase<Permission, PermissionListResponseDto, PermissionFilterDto> _paginationService;

    public GetAllPermissionsFiltered(
        IPermissionRepository permissionRepository,
        IPaginationServiceBase<Permission, PermissionListResponseDto, PermissionFilterDto> paginationService)
        => (_permissionRepository, _paginationService) = (permissionRepository, paginationService);

    public async Task<PaginationResponseDto<PermissionListResponseDto>> GetPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = _permissionRepository.QueryWithDetails();
        return await _paginationService.GetPaginatedAsync(baseQuery, filter, cancellationToken);
    }
}
