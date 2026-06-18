using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Notification;

public sealed class UserNotificationRepository(LiveEventsDbContext context)
    : IUserNotificationRepository
{
    private readonly LiveEventsDbContext _context = context;

    public IQueryable<UserNotification> QueryByUserId(Guid userId)
    {
        return _context.UserNotifications
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DeletedAt == null);
    }

    public Task<List<UserNotification>> ListByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _context.UserNotifications
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.DeletedAt == null)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<UserNotification?> GetForUserAsync(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        return _context.UserNotifications
            .FirstOrDefaultAsync(
                x => x.Id == notificationId && x.UserId == userId && x.DeletedAt == null,
                cancellationToken);
    }

    public Task<int> CountUnreadAsync(Guid userId, CancellationToken cancellationToken)
    {
        return _context.UserNotifications
            .CountAsync(
                x => x.UserId == userId &&
                     x.DeletedAt == null &&
                     x.Status != NotificationStatus.Read,
                cancellationToken);
    }

    public async Task<int> MarkAllAsReadAsync(Guid userId, DateTime readAt, CancellationToken cancellationToken)
    {
        var notifications = await _context.UserNotifications
            .Where(x => x.UserId == userId && x.DeletedAt == null && x.Status != NotificationStatus.Read)
            .ToListAsync(cancellationToken);

        foreach (var notification in notifications)
        {
            notification.Status = NotificationStatus.Read;
            notification.ReadAt = readAt;
            notification.UpdatedAt = readAt;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return notifications.Count;
    }
}
