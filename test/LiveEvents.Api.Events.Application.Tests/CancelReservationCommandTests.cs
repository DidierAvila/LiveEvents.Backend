using FluentAssertions;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.Tests.Support;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;
using NSubstitute;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class CancelReservationCommandTests
{
    private readonly IReservationRepository _reservationRepository = Substitute.For<IReservationRepository>();

    [Fact]
    public async Task HandleAsync_WhenConfirmedReservationIsCancelled_ShouldMarkItAsCancelled()
    {
        var command = new CancelReservation(_reservationRepository);
        var reservationId = Guid.NewGuid();
        var eventEntity = BuildEvent(x =>
        {
            x.Id = Guid.NewGuid();
            x.Title = "Concierto de cierre";
            x.StartsAt = DateTime.Now.AddHours(24);
            x.EndsAt = DateTime.Now.AddHours(26);
        });
        var reservationDetails = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.EventId = eventEntity.Id;
            x.Status = ReservationStatus.Confirmada;
            x.Event = eventEntity;
        });
        var reservation = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.EventId = eventEntity.Id;
            x.Status = ReservationStatus.Confirmada;
        });

        _reservationRepository.QueryWithEvent().Returns(new[] { reservationDetails }.AsAsyncQueryable());
        _reservationRepository.GetByID(reservationId, Arg.Any<CancellationToken>()).Returns(reservation);

        var result = await command.HandleAsync(reservationId, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Status.Should().Be(ReservationStatus.Cancelada);
        result.Value.CancelledAt.Should().NotBeNull();
        await _reservationRepository.Received(1).Update(
            Arg.Is<Reservation>(x => x.Id == reservationId && x.Status == ReservationStatus.Cancelada && x.CancelledAt.HasValue),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenReservationWasAlreadyCancelled_ShouldRejectRequest()
    {
        var command = new CancelReservation(_reservationRepository);
        var reservationId = Guid.NewGuid();
        var eventEntity = BuildEvent();
        var reservationDetails = BuildReservation(x =>
        {
            x.Id = reservationId;
            x.Status = ReservationStatus.Cancelada;
            x.Event = eventEntity;
        });

        _reservationRepository.QueryWithEvent().Returns(new[] { reservationDetails }.AsAsyncQueryable());

        var result = await command.HandleAsync(reservationId, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.AlreadyCancelled");
        await _reservationRepository.DidNotReceive().Update(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
    }
}
