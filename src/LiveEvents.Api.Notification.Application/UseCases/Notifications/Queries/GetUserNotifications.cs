using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Queries;

public sealed class GetUserNotifications(IUserNotificationRepository userNotificationRepository)
{
    private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;

    public async Task<Result<IEnumerable<UserNotificationDto>>> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<IEnumerable<UserNotificationDto>>(
                Error.Validation("Notification.UserRequired", "El usuario es obligatorio."));
        }

        var notifications = await _userNotificationRepository.ListByUserIdAsync(userId, cancellationToken);

        var result = notifications
            .Select(x => new UserNotificationDto
            {
                Id = x.Id,
                UserId = x.UserId,
                Title = x.Title,
                Message = x.Message,
                Channel = x.Channel,
                Type = x.Types,
                Status = x.Status,
                CreatedAt = x.CreatedAt,
                ReadAt = x.ReadAt,
                Metadata = x.Metadata
            })
            .ToList();

        return Result.Success<IEnumerable<UserNotificationDto>>(result);
    }
}
