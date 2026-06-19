using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Queries;

public class GetEventById
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetEventById(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<EventDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.QueryWithDetails()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (eventEntity is null)
        {
            return null;
        }

        var dto = _mapper.Map<EventDto>(eventEntity);
        dto.Status = EventStatusResolver.GetCurrentStatus(eventEntity);
        return dto;
    }
}
