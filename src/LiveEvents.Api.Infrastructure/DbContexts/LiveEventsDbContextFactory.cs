using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Infrastructure.DbContexts;

public sealed class LiveEventsDbContextFactory : IDesignTimeDbContextFactory<LiveEventsDbContext>
{
    public LiveEventsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No se encontro ConnectionStrings:DefaultConnection para diseno de migraciones.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<LiveEventsDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.MapEnum<UserStatus>("user_status", "authentication");
            npgsqlOptions.MapEnum<EventType>("events_types", "events");
            npgsqlOptions.MapEnum<EventStatus>("events_status", "events");
            npgsqlOptions.MapEnum<ReservationStatus>("reservations_status", "events");
            npgsqlOptions.CommandTimeout(120);
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });

        if (configuration.GetValue<bool>("EnableSensitiveDataLogging"))
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        return new LiveEventsDbContext(optionsBuilder.Options);
    }

    private static IConfiguration BuildConfiguration()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var candidatePaths = new[]
        {
            currentDirectory,
            Path.Combine(currentDirectory, "src", "LiveEvents.Api.Events"),
            Path.GetFullPath(Path.Combine(currentDirectory, "..", "LiveEvents.Api.Events")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LiveEvents.Api.Events"))
        };

        var configurationBuilder = new ConfigurationBuilder();

        foreach (var path in candidatePaths.Distinct().Where(Directory.Exists))
        {
            configurationBuilder
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true);
        }

        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder.Build();
    }
}
