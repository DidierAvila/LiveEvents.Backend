using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports.Notification;

namespace LiveEvents.Api.Notification.Application.Services;

public sealed class NotificationDispatcher(IEnumerable<INotificationChannelHandler> handlers)
    : INotificationDispatcher
{
    private readonly IReadOnlyDictionary<NotificationChannel, INotificationChannelHandler> _handlers =
        handlers.ToDictionary(x => x.Channel);

    public Task<UserNotification> DispatchAsync(NotificationDispatchRequest request, CancellationToken cancellationToken)
    {
        if (!_handlers.TryGetValue(request.Channel, out var handler))
        {
            throw new NotSupportedException($"No existe un manejador registrado para el canal {request.Channel}.");
        }

        return handler.SendAsync(request, cancellationToken);
    }
}
