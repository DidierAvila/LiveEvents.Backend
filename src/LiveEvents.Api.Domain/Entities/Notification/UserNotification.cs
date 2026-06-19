using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Notification;

[Table("user_notifications", Schema = "notification")]
public class UserNotification : BaseEntity
{
    public Guid UserId { get; set; }

    [MaxLength(150)]
    public required string Title { get; set; }

    [MaxLength(2000)]
    public required string Message { get; set; }

    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;
    public NotificationType Types { get; set; } = NotificationType.Generico;
    public NotificationStatus Status { get; set; } = NotificationStatus.Pendiente;
    public string? Metadata { get; set; }
    public DateTime? ReadAt { get; set; }
}
