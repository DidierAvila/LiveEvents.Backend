using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LiveEvents.Api.Common.Extensions;

public static class JwtAuthenticationExtensionsBase
{
    public static IServiceCollection AddJwtAuthenticationBase(this IServiceCollection services, IConfiguration configuration)
        => services.AddJwtAuthenticationBase(configuration, configureJwtBearer: null);

    public static IServiceCollection AddJwtAuthenticationBase(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<JwtBearerOptions>? configureJwtBearer)
    {
        var key = configuration.GetValue<string>("JwtSettings:key");
        var issuer = configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = configuration.GetValue<string>("JwtSettings:Audience");

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException("JwtSettings:key debe estar configurado.");
        }

        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("JwtSettings:Issuer y JwtSettings:Audience deben estar configurados.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(key);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            configureJwtBearer?.Invoke(options);
        });

        return services;
    }
}
