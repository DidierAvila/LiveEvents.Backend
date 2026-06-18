using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Queries;

public class GetAllUsersFiltered
{
    private readonly IUserRepository _userRepository;
    private readonly IPaginationServiceBase<User, UserBasicDto, UserFilterDto> _paginationService;

    public GetAllUsersFiltered(
        IUserRepository userRepository,
        IPaginationServiceBase<User, UserBasicDto, UserFilterDto> paginationService)
        => (_userRepository, _paginationService) = (userRepository, paginationService);

    public async Task<PaginationResponseDto<UserBasicDto>> HandleAsync(UserFilterDto filter, CancellationToken cancellationToken = default)
    {
        var baseQuery = _userRepository.QueryWithDetails();
        return await _paginationService.GetPaginatedAsync(baseQuery, filter, cancellationToken);
    }
}
