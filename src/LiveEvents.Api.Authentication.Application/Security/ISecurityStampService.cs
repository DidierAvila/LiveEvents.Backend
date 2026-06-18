using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.Security;

public interface ISecurityStampService
{
    Task<string> EnsureSecurityStampAsync(User user, CancellationToken cancellationToken);
    Task<string> RefreshSecurityStampAsync(User user, CancellationToken cancellationToken);
    Task<bool> RefreshSecurityStampAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> RefreshSecurityStampByRoleAsync(Guid roleId, CancellationToken cancellationToken);
}
