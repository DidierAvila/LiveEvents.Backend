using LiveEvents.Api.Domain.Entities.Notification;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;

public class CreateInAppNotificationDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; } = NotificationType.Generico;
    public Dictionary<string, object>? Metadata { get; set; }
}

public class UserNotificationDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? Metadata { get; set; }
}

public class UnreadNotificationsCountDto
{
    public Guid UserId { get; set; }
    public int UnreadCount { get; set; }
}
