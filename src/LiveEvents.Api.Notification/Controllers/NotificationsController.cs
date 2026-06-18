using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Queries;

namespace LiveEvents.Api.Notification.Controllers;

[Route("Api/[controller]")]
public class NotificationsController(
    CreateInAppNotification createInAppNotification,
    GetUserNotifications getUserNotifications,
    GetUnreadNotificationsCount getUnreadNotificationsCount,
    MarkNotificationAsRead markNotificationAsRead,
    MarkAllNotificationsAsRead markAllNotificationsAsRead) : ApiControllerBase
{
    private readonly CreateInAppNotification _createInAppNotification = createInAppNotification;
    private readonly GetUserNotifications _getUserNotifications = getUserNotifications;
    private readonly GetUnreadNotificationsCount _getUnreadNotificationsCount = getUnreadNotificationsCount;
    private readonly MarkNotificationAsRead _markNotificationAsRead = markNotificationAsRead;
    private readonly MarkAllNotificationsAsRead _markAllNotificationsAsRead = markAllNotificationsAsRead;

    [HttpPost("in-app")]
    public async Task<IActionResult> CreateInApp(
        [FromBody] CreateInAppNotificationDto request,
        CancellationToken cancellationToken)
    {
        var result = await _createInAppNotification.HandleAsync(request, cancellationToken);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetByUser), new { userId = result.Value.UserId }, result.Value)
            : HandleError(result.Error);
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _getUserNotifications.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("users/{userId:guid}/unread-count")]
    public async Task<IActionResult> GetUnreadCount(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _getUnreadNotificationsCount.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{notificationId:guid}/users/{userId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, Guid userId, CancellationToken cancellationToken)
    {
        var result = await _markNotificationAsRead.HandleAsync(notificationId, userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("users/{userId:guid}/read-all")]
    public async Task<IActionResult> MarkAllAsRead(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _markAllNotificationsAsRead.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }
}
