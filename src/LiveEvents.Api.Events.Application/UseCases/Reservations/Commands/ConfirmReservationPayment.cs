using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;

public class ConfirmReservationPayment
{
    private readonly IReservationRepository _reservationRepository;

    public ConfirmReservationPayment(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Result<ReservationDto>> HandleAsync(Guid reservationId, CancellationToken cancellationToken)
    {
        var reservation = await _reservationRepository.GetByID(reservationId, cancellationToken);
        if (reservation is null)
        {
            return Result.Failure<ReservationDto>(Error.NotFound("Reservation.NotFound", "La reserva indicada no existe."));
        }

        if (reservation.Status == ReservationStatus.Confirmada)
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.AlreadyConfirmed", "La reserva ya se encuentra confirmada."));
        }

        if (reservation.Status is ReservationStatus.Cancelada or ReservationStatus.Perdida)
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.CannotConfirm", "No se puede confirmar una reserva cancelada."));
        }

        reservation.Status = ReservationStatus.Confirmada;
        reservation.ReservationCode = await GenerateReservationCodeAsync(cancellationToken);
        reservation.PaidAt = DateTime.Now;
        reservation.UpdatedAt = DateTime.Now;

        await _reservationRepository.Update(reservation, cancellationToken);

        var updatedReservation = await _reservationRepository.QueryWithEvent()
            .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

        return Result.Success(new ReservationDto
        {
            Id = updatedReservation!.Id,
            EventId = updatedReservation.EventId,
            EventTitle = updatedReservation.Event?.Title,
            Quantity = updatedReservation.Quantity,
            BuyerName = updatedReservation.BuyerName,
            BuyerEmail = updatedReservation.BuyerEmail,
            Status = updatedReservation.Status,
            ReservationCode = updatedReservation.ReservationCode,
            CreatedAt = updatedReservation.CreatedAt,
            PaidAt = updatedReservation.PaidAt,
            CancelledAt = updatedReservation.CancelledAt
        });
    }

    private async Task<string> GenerateReservationCodeAsync(CancellationToken cancellationToken)
    {
        for (var attempt = 0; attempt < 20; attempt++)
        {
            var code = $"EV-{RandomNumberGenerator.GetInt32(0, 1_000_000):D6}";
            if (!await _reservationRepository.ReservationCodeExistsAsync(code, cancellationToken))
            {
                return code;
            }
        }

        throw new InvalidOperationException("No fue posible generar un codigo de reserva unico.");
    }
}
