using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.Utils;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Handlers;

namespace LiveEvents.Api.Events.Controllers;

[Route("Api/[controller]")]
public class ReservationsController(
    IReservationCommandHandler reservationCommandHandler,
    IReservationQueryHandler reservationQueryHandler) : ApiControllerBase
{
    private readonly IReservationCommandHandler _reservationCommandHandler = reservationCommandHandler;
    private readonly IReservationQueryHandler _reservationQueryHandler = reservationQueryHandler;

    [HttpGet("mine")]
    [Authorize]
    [ProducesResponseType(typeof(PaginationResponseDto<UserReservationListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetMine([FromQuery] UserReservationFilterDto filter, CancellationToken cancellationToken)
    {
        var buyerEmail = User.FindFirst(CustomClaimTypes.UserEmail)?.Value;
        if (string.IsNullOrWhiteSpace(buyerEmail))
        {
            return HandleError(Error.Unauthorized("Reservation.InvalidUser", "No fue posible identificar al usuario autenticado."));
        }

        var result = await _reservationQueryHandler.GetMyReservations(buyerEmail, filter, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReservationDto request,
        CancellationToken cancellationToken)
    {
        var result = await _reservationCommandHandler.CreateReservation(request, cancellationToken);
        return result.IsSuccess
            ? Created(string.Empty, result.Value)
            : HandleError(result.Error);
    }

    [HttpPut("{id:guid}/confirm-payment")]
    public async Task<IActionResult> ConfirmPayment(Guid id, CancellationToken cancellationToken)
    {
        var result = await _reservationCommandHandler.ConfirmReservationPayment(id, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await _reservationCommandHandler.CancelReservation(id, cancellationToken);
        return HandleResult(result);
    }
}
