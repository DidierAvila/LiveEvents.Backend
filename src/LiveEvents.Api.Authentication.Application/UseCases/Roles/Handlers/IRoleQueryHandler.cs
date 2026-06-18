using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Handlers;

public interface IRoleQueryHandler
{
    Task<Result<RoleDto>> GetRoleById(Guid id, CancellationToken cancellationToken);
    Task<Result<IEnumerable<RoleDto>>> GetAllRoles(CancellationToken cancellationToken);
    Task<Result<PaginationResponseDto<RoleListResponseDto>>> GetAllRolesFiltered(RoleFilterDto filter, CancellationToken cancellationToken);
    Task<Result<IEnumerable<RoleDropdownDto>>> GetRolesDropdown(CancellationToken cancellationToken);
}
