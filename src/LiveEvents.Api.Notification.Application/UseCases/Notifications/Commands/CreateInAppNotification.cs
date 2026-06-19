using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;

public sealed class CreateInAppNotification(
    IRepositoryBase<User> userRepository,
    INotificationDispatcher notificationDispatcher)
{
    private readonly IRepositoryBase<User> _userRepository = userRepository;
    private readonly INotificationDispatcher _notificationDispatcher = notificationDispatcher;

    public async Task<Result<UserNotificationDto>> HandleAsync(
        CreateInAppNotificationDto request,
        CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            return Result.Failure<UserNotificationDto>(
                Error.Validation("Notification.UserRequired", "El usuario es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return Result.Failure<UserNotificationDto>(
                Error.Validation("Notification.TitleRequired", "El título es obligatorio."));
        }

        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return Result.Failure<UserNotificationDto>(
                Error.Validation("Notification.MessageRequired", "El mensaje es obligatorio."));
        }

        var user = await _userRepository.Find(x => x.Id == request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserNotificationDto>(
                Error.NotFound("Notification.UserNotFound", "El usuario no existe."));
        }

        try
        {
            var notification = await _notificationDispatcher.DispatchAsync(
                new NotificationDispatchRequest
                {
                    UserId = request.UserId,
                    Title = request.Title.Trim(),
                    Message = request.Message.Trim(),
                    Types = request.Type,
                    Channel = NotificationChannel.InApp,
                    Metadata = request.Metadata
                },
                cancellationToken);

            return Result.Success(Map(notification));
        }
        catch (NotSupportedException ex)
        {
            return Result.Failure<UserNotificationDto>(
                Error.Failure("Notification.ChannelNotSupported", ex.Message));
        }
    }

    private static UserNotificationDto Map(UserNotification notification)
    {
        return new UserNotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            Channel = notification.Channel,
            Type = notification.Types,
            Status = notification.Status,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt,
            Metadata = notification.Metadata
        };
    }
}
