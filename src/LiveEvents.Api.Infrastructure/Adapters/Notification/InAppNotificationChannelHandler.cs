using System.Text.Json;
using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Notification;

namespace LiveEvents.Api.Infrastructure.Adapters.Notification;

public sealed class InAppNotificationChannelHandler(IRepositoryBase<UserNotification> repository)
    : INotificationChannelHandler
{
    private readonly IRepositoryBase<UserNotification> _repository = repository;

    public NotificationChannel Channel => NotificationChannel.InApp;

    public Task<UserNotification> SendAsync(NotificationDispatchRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        var notification = new UserNotification
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Channel = request.Channel,
            Types = request.Types,
            Status = NotificationStatus.Enviado,
            Metadata = request.Metadata is null ? null : JsonSerializer.Serialize(request.Metadata),
            CreatedAt = now,
            UpdatedAt = now
        };

        return _repository.Create(notification, cancellationToken);
    }
}
