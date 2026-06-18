using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Handlers
{
    public interface IUserQueryHandler
    {
        Task<Result<UserDto>> GetUserById(Guid id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserDto>>> GetAllUsers(CancellationToken cancellationToken);
        Task<Result<IEnumerable<UserBasicDto>>> GetAllUsersBasic(CancellationToken cancellationToken);
        Task<Result<PaginationResponseDto<UserBasicDto>>> GetAllUsersFiltered(UserFilterDto filter, CancellationToken cancellationToken);
        Task<UserMeResponseDto> GetUserMe(Guid userId, CancellationToken cancellationToken);
    }
}
