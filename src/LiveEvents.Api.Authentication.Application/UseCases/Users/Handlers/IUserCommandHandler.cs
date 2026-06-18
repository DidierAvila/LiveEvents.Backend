using LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Handlers;

public interface IUserCommandHandler
{
    Task<Result<UserDto>> CreateUser(CreateUserDto command, CancellationToken cancellationToken);
    Task<Result<UserDto>> UpdateUser(Guid id, UpdateUserDto command, CancellationToken cancellationToken);
    Task<Result> DeleteUser(Guid id, CancellationToken cancellationToken);
    Task<Result> ChangePassword(Guid userId, ChangePasswordDto command, CancellationToken cancellationToken);

    // Role management methods
    Task<Result<MultipleRoleAssignmentResult>> AssignMultipleRolesToUser(AssignMultipleRolesToUser command, CancellationToken cancellationToken);
    Task<Result<MultipleRoleRemovalResult>> RemoveMultipleRolesFromUser(RemoveMultipleRolesFromUser command, CancellationToken cancellationToken);
    Task<Result<List<UserRoleDto>>> GetUserRoles(Guid userId, CancellationToken cancellationToken);
}
