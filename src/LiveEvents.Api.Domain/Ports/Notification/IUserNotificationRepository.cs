using LiveEvents.Api.Domain.Entities.Notification;

namespace LiveEvents.Api.Domain.Ports.Notification;

public interface IUserNotificationRepository
{
    IQueryable<UserNotification> QueryByUserId(Guid userId);
    Task<List<UserNotification>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<UserNotification?> GetForUserAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken);
    Task<int> CountUnreadAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> MarkAllAsReadAsync(Guid userId, DateTime readAt, CancellationToken cancellationToken);
}
