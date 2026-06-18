using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;

public class GetAllRolesFiltered
{
    private readonly IRoleRepository _roleRepository;
    private readonly IPaginationServiceBase<Role, RoleListResponseDto, RoleFilterDto> _paginationService;

    public GetAllRolesFiltered(
        IRoleRepository roleRepository,
        IPaginationServiceBase<Role, RoleListResponseDto, RoleFilterDto> paginationService)
        => (_roleRepository, _paginationService) = (roleRepository, paginationService);

    public async Task<PaginationResponseDto<RoleListResponseDto>> GetRolesFiltered(RoleFilterDto filter, CancellationToken cancellationToken)
    {
        var basequery = _roleRepository.QueryWithDetails();
        return await _paginationService.GetPaginatedAsync(basequery, filter, cancellationToken);
    }
}
