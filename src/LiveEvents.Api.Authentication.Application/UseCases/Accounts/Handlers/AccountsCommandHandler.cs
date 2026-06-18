using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;

public class AccountsCommandHandler : IAccountsCommandHandler
{
    private readonly ChangePassword _changePassword;
    private readonly LoginCommand _loginCommand;
    private readonly TokenCommand _tokenCommand;


    public AccountsCommandHandler(ChangePassword changePassword, LoginCommand loginCommand, TokenCommand tokenCommand)
    {
        _changePassword = changePassword;
        _loginCommand = loginCommand;
        _tokenCommand = tokenCommand;
    }

    public async Task<Result> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        return await _changePassword.HandleAsync(userId, changePasswordDto, cancellationToken);
    }

    public async Task<string> GetToken(User user, CancellationToken cancellationToken)
    {
        return await _tokenCommand.HandleAsync(user, cancellationToken);
    }

    public async Task<Result<LoginResponseDto>> Login(LoginRequestDto autorizacion, CancellationToken cancellationToken)
    {
        return await _loginCommand.HandleAsync(autorizacion, cancellationToken);
    }
}
