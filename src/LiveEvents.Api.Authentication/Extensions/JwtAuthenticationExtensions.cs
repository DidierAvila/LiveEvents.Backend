using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using System.Text;

namespace LiveEvents.Api.Authentication.Extensions;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
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

            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var principal = context.Principal;
                    var userIdClaim = principal?.FindFirst(CustomClaimTypes.UserId)?.Value;
                    var securityStampClaim = principal?.FindFirst(CustomClaimTypes.SecurityStamp)?.Value;

                    if (string.IsNullOrWhiteSpace(userIdClaim) ||
                        string.IsNullOrWhiteSpace(securityStampClaim) ||
                        !Guid.TryParse(userIdClaim, out var userId))
                    {
                        context.Fail("Token inválido");
                        return;
                    }

                    var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepositoryBase<User>>();
                    var user = await userRepository.Find(u => u.Id == userId, context.HttpContext.RequestAborted);

                    if (user is null || user.Status != UserStatus.Activo)
                    {
                        context.Fail("Usuario inválido");
                        return;
                    }

                    var currentSecurityStamp = user.GetAdditionalValue<string>(CustomClaimTypes.SecurityStamp);
                    if (string.IsNullOrWhiteSpace(currentSecurityStamp) ||
                        !string.Equals(currentSecurityStamp, securityStampClaim, StringComparison.Ordinal))
                    {
                        context.Fail("Security stamp inválido");
                    }
                }
            };
        });

        return services;
    }
}
