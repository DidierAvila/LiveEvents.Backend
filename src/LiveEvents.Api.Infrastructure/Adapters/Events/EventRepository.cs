using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Events;

public class EventRepository : RepositoryBase<Event>, IEventRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public EventRepository(LiveEventsDbContext context, ILogger<EventRepository> logger)
        : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<Event> QueryWithDetails()
    {
        return _dbContext.Events
            .AsNoTracking()
            .Include(e => e.Venue);
    }

    public async Task<bool> HasOverlappingActiveEventAsync(
        Guid venueId,
        DateTime startsAt,
        DateTime endsAt,
        Guid? excludedEventId,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Events
            .AsNoTracking()
            .Where(e => e.VenueId == venueId && e.Status != EventStatus.Cancelado)
            .Where(e => e.StartsAt < endsAt && startsAt < e.EndsAt);

        if (excludedEventId.HasValue)
        {
            query = query.Where(e => e.Id != excludedEventId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
