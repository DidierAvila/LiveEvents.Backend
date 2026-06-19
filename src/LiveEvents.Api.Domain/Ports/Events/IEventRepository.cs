using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Domain.Ports.Events;

public interface IEventRepository : IRepositoryBase<Event>
{
    IQueryable<Event> QueryWithDetails();
    Task<bool> HasOverlappingActiveEventAsync(Guid venueId, DateTime startsAt, DateTime endsAt, Guid? excludedEventId, CancellationToken cancellationToken);
}
