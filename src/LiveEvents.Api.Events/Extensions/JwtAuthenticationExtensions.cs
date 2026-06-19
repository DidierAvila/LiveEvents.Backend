using Microsoft.AspNetCore.Authentication.JwtBearer;
using LiveEvents.Api.Common.Extensions;
using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Events.Extensions;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        => services.AddJwtAuthenticationBase(configuration, options =>
        {
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
}
