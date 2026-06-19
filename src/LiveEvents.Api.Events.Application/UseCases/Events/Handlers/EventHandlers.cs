using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Commands;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Queries;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Handlers;

public interface IEventCommandHandler
{
    Task<Result<EventDto>> CreateEvent(CreateEventDto command, CancellationToken cancellationToken);
}

public interface IEventQueryHandler
{
    Task<Result<EventDto>> GetEventById(Guid id, CancellationToken cancellationToken);
    Task<Result<PaginationResponseDto<EventListDto>>> GetAllEventsFiltered(EventFilterDto filter, CancellationToken cancellationToken);
    Task<Result<OccupationReportDto>> GetOccupationReport(Guid eventId, CancellationToken cancellationToken);
    Task<Result<IEnumerable<EventDropdownDto>>> GetEventsForDropdown(CancellationToken cancellationToken);
}

public class EventCommandHandler : IEventCommandHandler
{
    private readonly CreateEvent _createEvent;

    public EventCommandHandler(CreateEvent createEvent)
    {
        _createEvent = createEvent;
    }

    public Task<Result<EventDto>> CreateEvent(CreateEventDto command, CancellationToken cancellationToken)
    {
        return _createEvent.HandleAsync(command, cancellationToken);
    }
}

public class EventQueryHandler : IEventQueryHandler
{
    private readonly GetEventById _getEventById;
    private readonly GetAllEventsFiltered _getAllEventsFiltered;
    private readonly GetOccupationReport _getOccupationReport;
    private readonly GetEventsForDropdown _getEventsForDropdown;
    private readonly IValidationService _validationService;

    public EventQueryHandler(
        GetEventById getEventById,
        GetAllEventsFiltered getAllEventsFiltered,
        GetOccupationReport getOccupationReport,
        GetEventsForDropdown getEventsForDropdown,
        IValidationService validationService)
    {
        _getEventById = getEventById;
        _getAllEventsFiltered = getAllEventsFiltered;
        _getOccupationReport = getOccupationReport;
        _getEventsForDropdown = getEventsForDropdown;
        _validationService = validationService;
    }

    public async Task<Result<EventDto>> GetEventById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var eventDto = await _getEventById.HandleAsync(id, cancellationToken);
            return eventDto is null
                ? Result.Failure<EventDto>(Error.NotFound("Event.NotFound", $"Evento con ID {id} no encontrado"))
                : Result.Success(eventDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<EventDto>(Error.Failure("Event.GetById", ex.Message));
        }
    }

    public async Task<Result<PaginationResponseDto<EventListDto>>> GetAllEventsFiltered(EventFilterDto filter, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(
                filter,
                "Event.InvalidFilter",
                "Los filtros de eventos no son validos.",
                cancellationToken);
            if (validationResult.IsFailure)
            {
                return Result.Failure<PaginationResponseDto<EventListDto>>(validationResult.Error);
            }

            var events = await _getAllEventsFiltered.HandleAsync(filter, cancellationToken);
            return Result.Success(events);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PaginationResponseDto<EventListDto>>(Error.Validation("Event.InvalidFilter", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginationResponseDto<EventListDto>>(Error.Failure("Event.GetFiltered", ex.Message));
        }
    }

    public async Task<Result<OccupationReportDto>> GetOccupationReport(Guid eventId, CancellationToken cancellationToken)
    {
        try
        {
            var report = await _getOccupationReport.HandleAsync(eventId, cancellationToken);
            return report is null
                ? Result.Failure<OccupationReportDto>(Error.NotFound("Event.NotFound", $"Evento con ID {eventId} no encontrado"))
                : Result.Success(report);
        }
        catch (Exception ex)
        {
            return Result.Failure<OccupationReportDto>(Error.Failure("Event.GetOccupationReport", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<EventDropdownDto>>> GetEventsForDropdown(CancellationToken cancellationToken)
    {
        try
        {
            var events = await _getEventsForDropdown.HandleAsync(cancellationToken);
            return Result.Success(events);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<EventDropdownDto>>(Error.Failure("Event.GetDropdown", ex.Message));
        }
    }
}
