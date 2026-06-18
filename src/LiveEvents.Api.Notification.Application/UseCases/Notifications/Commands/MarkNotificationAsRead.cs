using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;

public sealed class MarkNotificationAsRead(
    IUserNotificationRepository userNotificationRepository,
    IRepositoryBase<UserNotification> repository)
{
    private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;
    private readonly IRepositoryBase<UserNotification> _repository = repository;

    public async Task<Result<UserNotificationDto>> HandleAsync(
        Guid notificationId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<UserNotificationDto>(
                Error.Validation("Notification.UserRequired", "El usuario es obligatorio."));
        }

        var notification = await _userNotificationRepository.GetForUserAsync(notificationId, userId, cancellationToken);
        if (notification is null)
        {
            return Result.Failure<UserNotificationDto>(
                Error.NotFound("Notification.NotFound", "La notificación no existe para el usuario indicado."));
        }

        if (notification.Status != NotificationStatus.Read)
        {
            var now = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
            notification.Status = NotificationStatus.Read;
            notification.ReadAt = now;
            notification.UpdatedAt = now;

            await _repository.Update(notification, cancellationToken);
        }

        return Result.Success(new UserNotificationDto
        {
            Id = notification.Id,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            Channel = notification.Channel,
            Type = notification.Type,
            Status = notification.Status,
            CreatedAt = notification.CreatedAt,
            ReadAt = notification.ReadAt,
            Metadata = notification.Metadata
        });
    }
}
