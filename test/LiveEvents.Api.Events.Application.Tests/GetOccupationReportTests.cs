using FluentAssertions;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.Tests.Support;
using LiveEvents.Api.Events.Application.UseCases.Events.Queries;
using NSubstitute;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class GetOccupationReportTests
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly IReservationRepository _reservationRepository = Substitute.For<IReservationRepository>();

    [Fact]
    public async Task HandleAsync_WhenEventExists_ShouldCalculateAvailabilityPercentageAndRevenue()
    {
        var query = new GetOccupationReport(_eventRepository, _reservationRepository);
        var eventId = Guid.NewGuid();
        var eventEntity = BuildEvent(x =>
        {
            x.Id = eventId;
            x.Title = "Reporte de ocupacion";
            x.MaxCapacity = 100;
            x.TicketPrice = 75.5m;
            x.StartsAt = DateTime.Now.AddDays(2);
            x.EndsAt = DateTime.Now.AddDays(2).AddHours(2);
        });

        _eventRepository.QueryWithDetails().Returns(new[] { eventEntity }.AsAsyncQueryable());
        _reservationRepository.GetConfirmedSeatsForEventAsync(eventId, Arg.Any<CancellationToken>()).Returns(40);
        _reservationRepository.GetUnavailableSeatsForEventAsync(eventId, Arg.Any<CancellationToken>()).Returns(55);

        var result = await query.HandleAsync(eventId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.ConfirmedTickets.Should().Be(40);
        result.AvailableTickets.Should().Be(45);
        result.OccupancyPercentage.Should().Be(40m);
        result.TotalRevenue.Should().Be(3020m);
    }

    [Fact]
    public async Task HandleAsync_WhenEventDoesNotExist_ShouldReturnNull()
    {
        var query = new GetOccupationReport(_eventRepository, _reservationRepository);

        _eventRepository.QueryWithDetails().Returns(Array.Empty<LiveEvents.Api.Domain.Entities.Events.Event>().AsAsyncQueryable());

        var result = await query.HandleAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeNull();
    }
}
