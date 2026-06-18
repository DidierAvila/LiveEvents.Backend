using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;

namespace LiveEvents.Api.Notification.Application.UseCases.Notifications.Queries;

public sealed class GetUnreadNotificationsCount(IUserNotificationRepository userNotificationRepository)
{
    private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;

    public async Task<Result<UnreadNotificationsCountDto>> HandleAsync(Guid userId, CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<UnreadNotificationsCountDto>(
                Error.Validation("Notification.UserRequired", "El usuario es obligatorio."));
        }

        var unreadCount = await _userNotificationRepository.CountUnreadAsync(userId, cancellationToken);
        return Result.Success(new UnreadNotificationsCountDto
        {
            UserId = userId,
            UnreadCount = unreadCount
        });
    }
}
