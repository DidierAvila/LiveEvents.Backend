using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Commands;
using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;

public class TokenCommandHandler : ITokenCommandHandler
{
    private readonly TokenCommand _tokenCommand;

    public TokenCommandHandler(TokenCommand tokenCommand)
    {
        _tokenCommand = tokenCommand;
    }

    public async Task<string> GetToken(User user, CancellationToken cancellationToken)
    {
        return await _tokenCommand.HandleAsync(user, cancellationToken);
    }
}
