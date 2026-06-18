using LiveEvents.Api.Domain.Entities.Notification;

namespace LiveEvents.Api.Domain.Ports.Notification;

public interface INotificationDispatcher
{
    Task<UserNotification> DispatchAsync(NotificationDispatchRequest request, CancellationToken cancellationToken);
}
