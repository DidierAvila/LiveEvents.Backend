using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using LiveEvents.Api.Common.Utils;

namespace LiveEvents.Api.Common.PermissionAttribute;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class RequirePermissionFromControllerAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _action;

    public RequirePermissionFromControllerAttribute(string action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                success = false,
                message = "Usuario no autenticado"
            });
            return;
        }

        var permissionPrefix = ResolvePermissionPrefix(context);
        var permission = $"{permissionPrefix}.{_action}";
        var permissionClaims = context.HttpContext.User.FindAll(CustomClaimTypes.Permission).Select(c => c.Value);

        if (!permissionClaims.Contains(permission))
        {
            context.Result = new ForbidResult();
        }
    }

    private static string ResolvePermissionPrefix(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return controllerActionDescriptor.ControllerName.ToLowerInvariant();
        }

        if (context.RouteData.Values.TryGetValue("controller", out var controllerValue) &&
            controllerValue is not null &&
            !string.IsNullOrWhiteSpace(controllerValue.ToString()))
        {
            return controllerValue.ToString()!.ToLowerInvariant();
        }

        throw new InvalidOperationException(
            $"No se pudo resolver el prefijo de permiso para {nameof(RequirePermissionFromControllerAttribute)}. " +
            $"Asegúrate de que la acción sea una acción MVC de controlador y que exista route data.");
    }
}

public interface IPermissionPrefixProvider
{
    string PermissionPrefix { get; }
}
