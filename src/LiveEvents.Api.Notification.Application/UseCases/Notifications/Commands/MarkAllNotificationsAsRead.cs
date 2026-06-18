using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Ports.Notification;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;

public sealed class MarkAllNotificationsAsRead(IUserNotificationRepository userNotificationRepository)
{
    private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;

    public async Task<Result<int>> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<int>(
                Error.Validation("Notification.UserRequired", "El usuario es obligatorio."));
        }

        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        var updated = await _userNotificationRepository.MarkAllAsReadAsync(userId, now, cancellationToken);
        return Result.Success(updated);
    }
}
