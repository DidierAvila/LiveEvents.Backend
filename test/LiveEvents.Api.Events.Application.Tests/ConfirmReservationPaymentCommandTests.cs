using FluentAssertions;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.Tests.Support;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;
using NSubstitute;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class ConfirmReservationPaymentCommandTests
{
    private readonly IReservationRepository _reservationRepository = Substitute.For<IReservationRepository>();

    [Fact]
    public async Task HandleAsync_WhenReservationIsAlreadyConfirmed_ShouldRejectRequest()
    {
        var command = new ConfirmReservationPayment(_reservationRepository);
        var reservationId = Guid.NewGuid();
        var reservation = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.Status = ReservationStatus.Confirmada;
        });

        _reservationRepository.GetByID(reservationId, Arg.Any<CancellationToken>()).Returns(reservation);

        var result = await command.HandleAsync(reservationId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.AlreadyConfirmed");
        await _reservationRepository.DidNotReceive().Update(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenReservationIsCancelled_ShouldRejectRequest()
    {
        var command = new ConfirmReservationPayment(_reservationRepository);
        var reservationId = Guid.NewGuid();
        var reservation = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.Status = ReservationStatus.Cancelada;
        });

        _reservationRepository.GetByID(reservationId, Arg.Any<CancellationToken>()).Returns(reservation);

        var result = await command.HandleAsync(reservationId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.CannotConfirm");
        await _reservationRepository.DidNotReceive().Update(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenReservationCanBeConfirmed_ShouldAssignCodeAndPaidDate()
    {
        var command = new ConfirmReservationPayment(_reservationRepository);
        var reservationId = Guid.NewGuid();
        var eventEntity = BuildEvent(x => x.Title = "Evento confirmado");
        var reservation = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.Status = ReservationStatus.PendientePago;
            x.EventId = eventEntity.Id;
            x.ReservationCode = null;
            x.PaidAt = null;
        });
        var updatedReservation = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.Status = ReservationStatus.Confirmada;
            x.EventId = eventEntity.Id;
            x.Event = eventEntity;
            x.ReservationCode = "EV-123456";
            x.PaidAt = DateTime.Now;
        });

        _reservationRepository.GetByID(reservationId, Arg.Any<CancellationToken>()).Returns(reservation);
        _reservationRepository.ReservationCodeExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(false);
        _reservationRepository.QueryWithEvent().Returns(new[] { updatedReservation }.AsAsyncQueryable());

        var result = await command.HandleAsync(reservationId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(ReservationStatus.Confirmada);
        result.Value.ReservationCode.Should().StartWith("EV-");
        result.Value.PaidAt.Should().NotBeNull();
        await _reservationRepository.Received(1).Update(
            Arg.Is<Reservation>(x =>
                x.Id == reservationId &&
                x.Status == ReservationStatus.Confirmada &&
                x.ReservationCode != null &&
                x.PaidAt.HasValue),
            Arg.Any<CancellationToken>());
    }
}
