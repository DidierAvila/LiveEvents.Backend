using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Dtos;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Queries;

namespace LiveEvents.Api.Notification.Controllers;

[Route("Api/[controller]")]
[Authorize]
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
            ? Created(string.Empty, result.Value)
            : HandleError(result.Error);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userIdResult = GetAuthenticatedUserId();
        if (userIdResult.IsFailure)
        {
            return HandleError(userIdResult.Error);
        }

        var userId = userIdResult.Value;
        var result = await _getUserNotifications.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("mine/unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var userIdResult = GetAuthenticatedUserId();
        if (userIdResult.IsFailure)
        {
            return HandleError(userIdResult.Error);
        }

        var userId = userIdResult.Value;
        var result = await _getUnreadNotificationsCount.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, CancellationToken cancellationToken)
    {
        var userIdResult = GetAuthenticatedUserId();
        if (userIdResult.IsFailure)
        {
            return HandleError(userIdResult.Error);
        }

        var userId = userIdResult.Value;
        var result = await _markNotificationAsRead.HandleAsync(notificationId, userId, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("mine/read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var userIdResult = GetAuthenticatedUserId();
        if (userIdResult.IsFailure)
        {
            return HandleError(userIdResult.Error);
        }

        var userId = userIdResult.Value;
        var result = await _markAllNotificationsAsRead.HandleAsync(userId, cancellationToken);
        return HandleResult(result);
    }

    private Result<Guid> GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst(CustomClaimTypes.UserId)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Result.Failure<Guid>(
                Error.Unauthorized(
                    "Notification.InvalidUser",
                    "No fue posible identificar al usuario autenticado."));
        }

        return Result.Success(userId);
    }
}
