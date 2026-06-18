using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Infrastructure;
using LiveEvents.Api.Notification.Application.Services;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Commands;
using LiveEvents.Api.Notification.Application.UseCases.Notifications.Queries;

namespace LiveEvents.Api.Notification.Application;

public static class NotificationApplicationDependencyInjection
{
    public static IServiceCollection AddNotificationApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<INotificationDispatcher, NotificationDispatcher>();

        services.AddScoped<CreateInAppNotification>();
        services.AddScoped<MarkNotificationAsRead>();
        services.AddScoped<MarkAllNotificationsAsRead>();
        services.AddScoped<GetUserNotifications>();
        services.AddScoped<GetUnreadNotificationsCount>();

        services.AddInfrastructure(configuration);

        return services;
    }
}
