using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;
using LiveEvents.Api.Common.Controllers;

namespace LiveEvents.Api.Authentication.Controllers;

/// <summary>
/// Controlador para la autenticación de usuarios.
/// </summary>
[Route("Api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAccountsCommandHandler _accountsCommandHandler;

    public AuthController(
        ILogger<AuthController> logger,
        IAccountsCommandHandler accountsCommandHandler)
    {
        _logger = logger;
        _accountsCommandHandler = accountsCommandHandler;
    }

    /// <summary>
    /// Inicia sesión en el sistema con las credenciales proporcionadas.
    /// </summary>
    /// <param name="autorizacion">Objeto que contiene las credenciales de inicio de sesión (email y contraseña).</param>
    /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
    /// <returns>Un token JWT si las credenciales son válidas; de lo contrario, un error.</returns>
    /// <response code="200">Retorna el token JWT de autenticación</response>
    /// <response code="401">Credenciales inválidas</response>
    /// <response code="403">Usuario inactivo</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [Route("Login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto autorizacion, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for {Email}", autorizacion.Email);
        var result = await _accountsCommandHandler.Login(autorizacion, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result.Value.Token);
        }

        return HandleError(result.Error);
    }
}
