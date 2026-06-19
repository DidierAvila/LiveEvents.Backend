using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Entities.Notification;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Domain.Ports.Notification;
using LiveEvents.Api.Infrastructure.Adapters;
using LiveEvents.Api.Infrastructure.Adapters.Authentication;
using LiveEvents.Api.Infrastructure.Adapters.Events;
using LiveEvents.Api.Infrastructure.Adapters.Notification;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar DbContext usando el DataSource con enums mapeados
        services.AddDbContext<LiveEventsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
            {
                npgsqlOptions.MapEnum<UserStatus>("user_status", "authentication");
                npgsqlOptions.MapEnum<EventType>("events_types", "events");
                npgsqlOptions.MapEnum<EventStatus>("events_status", "events");
                npgsqlOptions.MapEnum<ReservationStatus>("reservations_status", "events");
                npgsqlOptions.CommandTimeout(120); // 2 minutos timeout para comandos
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorCodesToAdd: null);
            })
            .EnableServiceProviderCaching(false)
            .EnableSensitiveDataLogging(configuration.GetValue<bool>("EnableSensitiveDataLogging")));

        // Repositories
        services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
        services.AddScoped<IRepositoryBase<UserType>, RepositoryBase<UserType>>();
        services.AddScoped<IRepositoryBase<Session>, RepositoryBase<Session>>();
        services.AddScoped<IRepositoryBase<Role>, RepositoryBase<Role>>();
        services.AddScoped<IRepositoryBase<Permission>, RepositoryBase<Permission>>();
        services.AddScoped<IRepositoryBase<UserRole>, RepositoryBase<UserRole>>();
        services.AddScoped<IRepositoryBase<RolePermission>, RepositoryBase<RolePermission>>();
        services.AddScoped<IRepositoryBase<UserNotification>, RepositoryBase<UserNotification>>();
        services.AddScoped<IRepositoryBase<Venue>, RepositoryBase<Venue>>();
        services.AddScoped<IRepositoryBase<Event>, RepositoryBase<Event>>();
        services.AddScoped<IRepositoryBase<Reservation>, RepositoryBase<Reservation>>();
        services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
        services.AddScoped<IVenueRepository, VenueRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<INotificationChannelHandler, InAppNotificationChannelHandler>();

        return services;
    }
}
