using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;

public interface IAccountsCommandHandler
{
    Task<Result<LoginResponseDto>> Login(LoginRequestDto autorizacion, CancellationToken cancellationToken);
    Task<Result> ChangePassword(Guid userId, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken);
}