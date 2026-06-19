using AutoMapper;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Commands;

public class CreateEvent
{
    private readonly IEventRepository _eventRepository;
    private readonly IVenueRepository _venueRepository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CreateEvent(
        IEventRepository eventRepository,
        IVenueRepository venueRepository,
        IMapper mapper,
        IValidationService validationService)
    {
        _eventRepository = eventRepository;
        _venueRepository = venueRepository;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<Result<EventDto>> HandleAsync(CreateEventDto createEventDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateAsync(
            createEventDto,
            "Event.InvalidData",
            "Los datos del evento no son validos.",
            cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result.Failure<EventDto>(validationResult.Error);
        }

        var venue = await _venueRepository.Find(v => v.Id == createEventDto.VenueId && v.Status, cancellationToken);
        if (venue is null)
        {
            return Result.Failure<EventDto>(Error.NotFound("Venue.NotFound", "El venue indicado no existe o no esta disponible."));
        }

        if (createEventDto.MaxCapacity > venue.Capacity)
        {
            return Result.Failure<EventDto>(Error.Validation("Event.CapacityExceeded", "La capacidad maxima del evento no puede superar la capacidad del venue."));
        }

        if (IsWeekendAfterAllowedHour(createEventDto.StartsAt))
        {
            return Result.Failure<EventDto>(Error.Validation("Event.InvalidWeekendTime", "Los eventos en fin de semana no pueden iniciar despues de las 22:00."));
        }

        var hasOverlap = await _eventRepository.HasOverlappingActiveEventAsync(
            createEventDto.VenueId,
            createEventDto.StartsAt,
            createEventDto.EndsAt,
            excludedEventId: null,
            cancellationToken);

        if (hasOverlap)
        {
            return Result.Failure<EventDto>(Error.Conflict("Event.OverlappingVenue", "Ya existe un evento activo para ese venue en el rango de tiempo indicado."));
        }

        var eventEntity = _mapper.Map<LiveEvents.Api.Domain.Entities.Events.Event>(createEventDto);

        var createdEvent = await _eventRepository.Create(eventEntity, cancellationToken);

        var dto = _mapper.Map<EventDto>(createdEvent);
        dto.VenueName = venue.Name;
        dto.VenueCity = venue.City;
        dto.Status = EventStatusResolver.GetCurrentStatus(createdEvent);
        return Result.Success(dto);
    }

    private static bool IsWeekendAfterAllowedHour(DateTime startsAt)
    {
        var isWeekend = startsAt.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        return isWeekend && startsAt.TimeOfDay > new TimeSpan(22, 0, 0);
    }
}
