using AutoMapper;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;
using LiveEvents.Api.Infrastructure.Adapters.Pagination;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Queries;

public sealed class EventPaginationService : PaginationServiceBase<Event, EventListDto, EventFilterDto>
{
    private readonly IMapper _mapper;

    public EventPaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected override IQueryable<Event> ApplyFilters(IQueryable<Event> query, EventFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(e => e.Title.ToLower().Contains(search));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(e => e.Type == filter.Type.Value);
        }

        if (filter.StartsFrom.HasValue)
        {
            query = query.Where(e => e.StartsAt >= filter.StartsFrom.Value);
        }

        if (filter.StartsTo.HasValue)
        {
            query = query.Where(e => e.StartsAt <= filter.StartsTo.Value);
        }

        if (filter.VenueId.HasValue)
        {
            query = query.Where(e => e.VenueId == filter.VenueId.Value);
        }

        if (filter.Status.HasValue)
        {
            var now = DateTime.Now;
            query = filter.Status.Value switch
            {
                EventStatus.Cancelado => query.Where(e => e.Status == EventStatus.Cancelado),
                EventStatus.Completado => query.Where(e => e.Status == EventStatus.Completado || (e.Status != EventStatus.Cancelado && e.EndsAt < now)),
                _ => query.Where(e => e.Status != EventStatus.Cancelado && e.EndsAt >= now)
            };
        }

        return query;
    }

    protected override IQueryable<Event> ApplySorting(IQueryable<Event> query, string? sortBy, bool sortDescending)
    {
        return SortingHelper.CreateSortingBuilder(query)
            .AddSortMapping("title", e => e.Title)
            .AddSortMapping("startsat", e => e.StartsAt)
            .AddSortMapping("endsat", e => e.EndsAt)
            .AddSortMapping("ticketprice", e => e.TicketPrice)
            .AddSortMapping("maxcapacity", e => e.MaxCapacity)
            .AddSortMapping("createdat", e => e.CreatedAt)
            .AddSortMapping("venuename", e => e.Venue != null ? e.Venue.Name : string.Empty)
            .SetDefaultSort(e => e.StartsAt)
            .ApplySorting(sortBy, sortDescending);
    }

    protected override Task<IEnumerable<EventListDto>> MapToDto(IEnumerable<Event> entities, CancellationToken cancellationToken)
    {
        var source = entities.ToList();
        var dtos = _mapper.Map<List<EventListDto>>(source);

        for (var index = 0; index < source.Count; index++)
        {
            dtos[index].Status = EventStatusResolver.GetCurrentStatus(source[index]);
        }

        return Task.FromResult<IEnumerable<EventListDto>>(dtos);
    }
}
