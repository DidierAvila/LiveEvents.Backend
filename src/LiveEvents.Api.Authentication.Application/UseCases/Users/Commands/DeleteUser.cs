using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;

public class DeleteUser
{
    private readonly IRepositoryBase<User> _userRepository;

    public DeleteUser(IRepositoryBase<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        // Find existing user
        var user = await _userRepository.Find(x => x.Id == id, cancellationToken);
        if (user == null)
            return Result.Failure(Error.NotFound("User.NotFound", "User not found"));

        await _userRepository.Delete(user, cancellationToken);
        return Result.Success();
    }
}
