namespace LiveEvents.Api.Domain.Entities.Notification;

public enum NotificationChannel
{
    InApp = 1,
    Email = 2,
    Sms = 3,
    Push = 4
}

public enum NotificationType
{
    Generic = 1,
    Security = 2,
    System = 3
}

public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Read = 3,
    Failed = 4
}

public sealed class NotificationDispatchRequest
{
    public Guid UserId { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
    public NotificationChannel Channel { get; init; }
    public NotificationType Type { get; init; } = NotificationType.Generic;
    public Dictionary<string, object>? Metadata { get; init; }
}
