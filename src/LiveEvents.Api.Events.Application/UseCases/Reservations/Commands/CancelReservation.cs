using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;

public class CancelReservation
{
    private readonly IReservationRepository _reservationRepository;

    public CancelReservation(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Result<ReservationDto>> HandleAsync(Guid reservationId, CancellationToken cancellationToken)
    {
        var reservationDetails = await _reservationRepository.QueryWithEvent()
            .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

        if (reservationDetails is null)
        {
            return Result.Failure<ReservationDto>(Error.NotFound("Reservation.NotFound", "La reserva indicada no existe."));
        }

        if (reservationDetails.Status is ReservationStatus.Cancelada or ReservationStatus.Perdida)
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.AlreadyCancelled", "La reserva ya fue cancelada."));
        }

        var reservation = await _reservationRepository.GetByID(reservationId, cancellationToken);
        if (reservation is null)
        {
            return Result.Failure<ReservationDto>(Error.NotFound("Reservation.NotFound", "La reserva indicada no existe."));
        }

        var now = DateTime.Now;
        reservation.Status = ReservationStatus.Cancelada;
        reservation.CancelledAt = now;
        reservation.UpdatedAt = now;

        await _reservationRepository.Update(reservation, cancellationToken);

        reservationDetails.Status = reservation.Status;
        reservationDetails.CancelledAt = reservation.CancelledAt;

        return Result.Success(new ReservationDto
        {
            Id = reservationDetails.Id,
            EventId = reservationDetails.EventId,
            EventTitle = reservationDetails.Event?.Title,
            Quantity = reservationDetails.Quantity,
            BuyerName = reservationDetails.BuyerName,
            BuyerEmail = reservationDetails.BuyerEmail,
            Status = reservationDetails.Status,
            ReservationCode = reservationDetails.ReservationCode,
            CreatedAt = reservationDetails.CreatedAt,
            PaidAt = reservationDetails.PaidAt,
            CancelledAt = reservationDetails.CancelledAt
        });
    }
}
