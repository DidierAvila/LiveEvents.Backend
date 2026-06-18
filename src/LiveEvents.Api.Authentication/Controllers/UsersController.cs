using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Handlers;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.PermissionAttribute;

namespace LiveEvents.Api.Authentication.Controllers;

/// <summary>
/// Controlador para gestionar los usuarios del sistema.
/// </summary>
[Route("Api/[controller]")]
[Authorize]
public class UsersController : ServiceCrudControllerBase<PaginationResponseDto<UserBasicDto>, UserDto, CreateUserDto, UpdateUserDto, UserFilterDto>
{
    /// <summary>
    /// Cambia la contraseña de un usuario
    /// </summary>
    /// <param name="id"></param>
    /// <param name="changePasswordDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("{id}/change-password")]
    [RequirePermission("users.update")]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangePasswordDto changePasswordDto,
        [FromServices] IUserCommandHandler userCommandHandler,
        CancellationToken cancellationToken)
    {
        var result = await userCommandHandler.ChangePassword(id, changePasswordDto, cancellationToken);
        return HandleResult(result);
    }

    // ======================================
    // ROLE MANAGEMENT ENDPOINTS
    // ======================================

    /// <summary>
    /// Obtiene todos los roles asignados a un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Lista de roles del usuario</returns>
    [HttpGet("{id}/roles")]
    [RequirePermission("roles.read")]
    public async Task<IActionResult> GetUserRoles(
        Guid id,
        [FromServices] IUserCommandHandler userCommandHandler,
        CancellationToken cancellationToken)
    {
        var result = await userCommandHandler.GetUserRoles(id, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Asigna múltiples roles a un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="roleIds">Lista de IDs de roles a asignar</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Resultado de la asignación múltiple</returns>
    [HttpPost("{id}/roles")]
    [RequirePermission("users.manage")]
    public async Task<IActionResult> AssignRolesToUser(
        Guid id,
        [FromBody] List<Guid> roleIds,
        [FromServices] IUserCommandHandler userCommandHandler,
        CancellationToken cancellationToken)
    {
        var command = new AssignMultipleRolesToUser
        {
            UserId = id,
            RoleIds = roleIds
        };

        var result = await userCommandHandler.AssignMultipleRolesToUser(command, cancellationToken);
        return HandleResult(result);
    }

    /// <summary>
    /// Remueve múltiples roles de un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="roleIds">Lista de IDs de roles a remover</param>
    /// <param name="cancellationToken">Token de cancelación</param>
    /// <returns>Resultado de la remoción múltiple</returns>
    [HttpDelete("{id}/roles")]
    [RequirePermission("users.manage")]
    public async Task<IActionResult> RemoveRolesFromUser(
        Guid id,
        [FromBody] List<Guid> roleIds,
        [FromServices] IUserCommandHandler userCommandHandler,
        CancellationToken cancellationToken)
    {
        var command = new RemoveMultipleRolesFromUser
        {
            UserId = id,
            RoleIds = roleIds
        };

        var result = await userCommandHandler.RemoveMultipleRolesFromUser(command, cancellationToken);
        return HandleResult(result);
    }
}
