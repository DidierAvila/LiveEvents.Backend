using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveEvents.Api.Common.Extensions;

public static class CorsExtensions
{
    public const string AllowFrontendPolicy = "AllowFrontend";

    public static IServiceCollection AddFrontendCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("CorsSettings:AllowedOrigins").Get<string[]>()
                             ?? new[] { "http://localhost:4200" };

        services.AddCors(options =>
        {
            options.AddPolicy(AllowFrontendPolicy, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }
}
