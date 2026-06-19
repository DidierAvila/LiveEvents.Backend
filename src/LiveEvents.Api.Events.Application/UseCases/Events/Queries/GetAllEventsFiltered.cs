using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Queries;

public class GetAllEventsFiltered
{
    private readonly IEventRepository _eventRepository;
    private readonly IPaginationServiceBase<Event, EventListDto, EventFilterDto> _paginationService;

    public GetAllEventsFiltered(
        IEventRepository eventRepository,
        IPaginationServiceBase<Event, EventListDto, EventFilterDto> paginationService)
    {
        _eventRepository = eventRepository;
        _paginationService = paginationService;
    }

    public Task<PaginationResponseDto<EventListDto>> HandleAsync(EventFilterDto filter, CancellationToken cancellationToken)
    {
        return _paginationService.GetPaginatedAsync(_eventRepository.QueryWithDetails(), filter, cancellationToken);
    }
}
