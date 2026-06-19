using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Queries;

public class GetEventsForDropdown
{
    private readonly IEventRepository _eventRepository;

    public GetEventsForDropdown(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<IEnumerable<EventDropdownDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.Now;

        var events = await _eventRepository.QueryWithDetails()
            .Where(e => e.Status != EventStatus.Cancelado && e.EndsAt > now)
            .OrderBy(e => e.StartsAt)
            .ToListAsync(cancellationToken);

        return events
            .Select(eventEntity => new EventDropdownDto
            {
                Id = eventEntity.Id,
                Name = $"{eventEntity.Title} - {eventEntity.StartsAt:yyyy-MM-dd HH:mm}"
            })
            .ToList();
    }
}
