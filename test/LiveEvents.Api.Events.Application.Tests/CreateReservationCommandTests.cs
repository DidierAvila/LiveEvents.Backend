using AutoMapper;
using FluentAssertions;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;
using LiveEvents.Api.Events.Application.Validation;
using NSubstitute;
using static LiveEvents.Api.Events.Application.Tests.Support.EventsTestData;

namespace LiveEvents.Api.Events.Application.Tests;

public class CreateReservationCommandTests
{
    private readonly IEventRepository _eventRepository = Substitute.For<IEventRepository>();
    private readonly IReservationRepository _reservationRepository = Substitute.For<IReservationRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidationService _validationService = Substitute.For<IValidationService>();

    [Fact]
    public async Task HandleAsync_WhenEventStartsInLessThanOneHour_ShouldRejectRuleRn04()
    {
        var command = new CreateReservation(_eventRepository, _reservationRepository, _mapper, _validationService);
        var dto = BuildCreateReservationDto();
        var eventEntity = BuildEvent(x =>
        {
            x.Id = dto.EventId;
            x.StartsAt = DateTime.Now.AddMinutes(45);
            x.EndsAt = DateTime.Now.AddHours(2);
            x.Status = EventStatus.Activo;
        });

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventRepository.GetByID(dto.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.TooLate");
        result.Error.Type.Should().Be(ErrorType.Conflict);
        await _reservationRepository.DidNotReceive().Create(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEventPriceIsGreaterThanOneHundredAndQuantityExceedsTen_ShouldRejectRuleRn05()
    {
        var command = new CreateReservation(_eventRepository, _reservationRepository, _mapper, _validationService);
        var dto = BuildCreateReservationDto(x => x.Quantity = 11);
        var eventEntity = BuildEvent(x =>
        {
            x.Id = dto.EventId;
            x.StartsAt = DateTime.Now.AddDays(3);
            x.EndsAt = DateTime.Now.AddDays(3).AddHours(2);
            x.TicketPrice = 150;
            x.Status = EventStatus.Activo;
        });

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventRepository.GetByID(dto.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.MaxTenEntries");
        result.Error.Type.Should().Be(ErrorType.Validation);
        await _reservationRepository.DidNotReceive().GetUnavailableSeatsForEventAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEventStartsInLessThanTwentyFourHours_AndQuantityExceedsFive_ShouldRejectBusinessRule()
    {
        var command = new CreateReservation(_eventRepository, _reservationRepository, _mapper, _validationService);
        var dto = BuildCreateReservationDto(x => x.Quantity = 6);
        var eventEntity = BuildEvent(x =>
        {
            x.Id = dto.EventId;
            x.StartsAt = DateTime.Now.AddHours(12);
            x.EndsAt = DateTime.Now.AddHours(14);
            x.TicketPrice = 90;
            x.Status = EventStatus.Activo;
        });

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventRepository.GetByID(dto.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.MaxFiveEntries");
        result.Error.Type.Should().Be(ErrorType.Validation);
    }

    [Fact]
    public async Task HandleAsync_WhenEventIsCompleted_ShouldRejectReservation()
    {
        var command = new CreateReservation(_eventRepository, _reservationRepository, _mapper, _validationService);
        var dto = BuildCreateReservationDto();
        var eventEntity = BuildEvent(x =>
        {
            x.Id = dto.EventId;
            x.StartsAt = DateTime.Now.AddHours(-3);
            x.EndsAt = DateTime.Now.AddHours(-1);
            x.Status = EventStatus.Activo;
        });

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventRepository.GetByID(dto.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.EventUnavailable");
        result.Error.Type.Should().Be(ErrorType.Conflict);
    }

    [Fact]
    public async Task HandleAsync_WhenRequestedTicketsExceedAvailableCapacity_ShouldRejectReservation()
    {
        var command = new CreateReservation(_eventRepository, _reservationRepository, _mapper, _validationService);
        var dto = BuildCreateReservationDto(x => x.Quantity = 15);
        var eventEntity = BuildEvent(x =>
        {
            x.Id = dto.EventId;
            x.MaxCapacity = 20;
            x.StartsAt = DateTime.Now.AddDays(2);
            x.EndsAt = DateTime.Now.AddDays(2).AddHours(2);
            x.Status = EventStatus.Activo;
        });

        _validationService.ValidateAsync(dto, Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success());
        _eventRepository.GetByID(dto.EventId, Arg.Any<CancellationToken>()).Returns(eventEntity);
        _reservationRepository.GetUnavailableSeatsForEventAsync(dto.EventId, Arg.Any<CancellationToken>()).Returns(10);

        var result = await command.HandleAsync(dto, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Reservation.NoAvailability");
        result.Error.Type.Should().Be(ErrorType.Conflict);
        await _reservationRepository.DidNotReceive().Create(Arg.Any<Reservation>(), Arg.Any<CancellationToken>());
    }
}
