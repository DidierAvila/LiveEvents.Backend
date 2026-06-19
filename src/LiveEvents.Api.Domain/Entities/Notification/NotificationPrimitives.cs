using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;

namespace LiveEvents.Api.Domain.Entities.Notification;

public enum NotificationChannel
{
    [PgName("InApp")]
    [Display(Name = "InApp")]
    InApp = 1,

    [PgName("Email")]
    [Display(Name = "Email")]
    Email = 2,

    [PgName("Sms")]
    [Display(Name = "Sms")]
    Sms = 3,

    [PgName("Push")]
    [Display(Name = "Push")]
    Push = 4
}

public enum NotificationType
{
    [PgName("Generico")]
    [Display(Name = "Genérico")]
    Generico = 1,

    [PgName("Seguridad")]
    [Display(Name = "Seguridad")]
    Seguridad = 2,

    [PgName("Sistema")]
    [Display(Name = "Sistema")]
    Sistema = 3
}

public enum NotificationStatus
{
    [PgName("Pendiente")]
    [Display(Name = "Pendiente")]
    Pendiente = 1,

    [PgName("Enviado")]
    [Display(Name = "Enviado")]
    Enviado = 2,

    [PgName("Leido")]
    [Display(Name = "Leído")]
    Leido = 3,

    [PgName("Fallido")]
    [Display(Name = "Fallido")]
    Fallido = 4
}

public sealed class NotificationDispatchRequest
{
    public Guid UserId { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
    public NotificationChannel Channel { get; init; }
    public NotificationType Types { get; init; } = NotificationType.Generico;
    public Dictionary<string, object>? Metadata { get; init; }
}
