using AutoMapper;
using FluentAssertions;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Commands;
using LiveEvents.Api.Events.Application.Validation;
using NSubstitute;
using System.Linq.Expressions;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class CreateEventCommandTests
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly IVenueRepository _venueRepository = Substitute.For<IVenueRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidationService _validationService = Substitute.For<IValidationService>();

    [Fact]
    public async Task HandleAsync_WhenEventCapacityExceedsVenueCapacity_ShouldRejectRuleRn01()
    {
        var command = new CreateEvent(_eventRepository, _venueRepository, _mapper, _validationService);
        var dto = BuildCreateEventDto(x => x.MaxCapacity = 120);
        var venue = BuildVenue(x => x.Capacity = 100);

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _venueRepository.Find(Arg.Any<Expression<Func<Venue, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(venue);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Event.CapacityExceeded");
        result.Error.Type.Should().Be(ErrorType.Validation);
        await _eventRepository.DidNotReceive().Create(Arg.Any<Event>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenVenueHasOverlappingActiveEvent_ShouldRejectRuleRn02()
    {
        var command = new CreateEvent(_eventRepository, _venueRepository, _mapper, _validationService);
        var dto = BuildCreateEventDto();
        var venue = BuildVenue(x => x.Id = dto.VenueId);

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _venueRepository.Find(Arg.Any<Expression<Func<Venue, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(venue);
        _eventRepository.HasOverlappingActiveEventAsync(dto.VenueId, dto.StartsAt, dto.EndsAt, null, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Event.OverlappingVenue");
        result.Error.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task HandleAsync_WhenWeekendEventStartsAfterTenPm_ShouldRejectRuleRn03()
    {
        var command = new CreateEvent(_eventRepository, _venueRepository, _mapper, _validationService);
        var startsAt = NextSaturdayAt(22, 1);
        var dto = BuildCreateEventDto(x =>
        {
            x.StartsAt = startsAt;
            x.EndsAt = startsAt.AddHours(2);
        });
        var venue = BuildVenue(x => x.Id = dto.VenueId);

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _venueRepository.Find(Arg.Any<Expression<Func<Venue, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(venue);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Event.InvalidWeekendTime");
        result.Error.Type.Should().Be(ErrorType.Validation);
        await _eventRepository.DidNotReceive().HasOverlappingActiveEventAsync(
            Arg.Any<Guid>(),
            Arg.Any<DateTime>(),
            Arg.Any<DateTime>(),
            Arg.Any<Guid?>(),
            Arg.Any<CancellationToken>());
    }
}
