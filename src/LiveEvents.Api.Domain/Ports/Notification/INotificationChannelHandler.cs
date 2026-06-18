using LiveEvents.Api.Domain.Entities.Notification;

namespace LiveEvents.Api.Domain.Ports.Notification;

public interface INotificationChannelHandler
{
    NotificationChannel Channel { get; }
    Task<UserNotification> SendAsync(NotificationDispatchRequest request, CancellationToken cancellationToken);
}
