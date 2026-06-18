using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.Security;

public class SecurityStampService : ISecurityStampService
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public SecurityStampService(IRepositoryBase<User> userRepository, IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<string> EnsureSecurityStampAsync(User user, CancellationToken cancellationToken)
    {
        var currentSecurityStamp = user.GetAdditionalValue<string>(CustomClaimTypes.SecurityStamp);
        if (!string.IsNullOrWhiteSpace(currentSecurityStamp))
        {
            return currentSecurityStamp;
        }

        return await SetNewSecurityStampAsync(user, cancellationToken);
    }

    public Task<string> RefreshSecurityStampAsync(User user, CancellationToken cancellationToken)
    {
        return SetNewSecurityStampAsync(user, cancellationToken);
    }

    public async Task<bool> RefreshSecurityStampAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Find(u => u.Id == userId, cancellationToken);
        if (user == null)
        {
            return false;
        }

        await SetNewSecurityStampAsync(user, cancellationToken);
        return true;
    }

    public async Task<int> RefreshSecurityStampByRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.GetRoleUsersAsync(roleId, cancellationToken);
        var affectedUserIds = userRoles
            .Select(x => x.UserId)
            .Distinct()
            .ToList();

        var updatedUsers = 0;
        foreach (var userId in affectedUserIds)
        {
            if (await RefreshSecurityStampAsync(userId, cancellationToken))
            {
                updatedUsers++;
            }
        }

        return updatedUsers;
    }

    private async Task<string> SetNewSecurityStampAsync(User user, CancellationToken cancellationToken)
    {
        var securityStamp = Guid.NewGuid().ToString("N");
        user.SetAdditionalValue(CustomClaimTypes.SecurityStamp, securityStamp);
        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        await _userRepository.Update(user, cancellationToken);
        return securityStamp;
    }
}
