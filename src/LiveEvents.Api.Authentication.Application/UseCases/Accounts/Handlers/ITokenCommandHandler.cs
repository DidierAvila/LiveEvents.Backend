using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;

public interface ITokenCommandHandler
{
    Task<string> GetToken(User user, CancellationToken cancellationToken);
}
