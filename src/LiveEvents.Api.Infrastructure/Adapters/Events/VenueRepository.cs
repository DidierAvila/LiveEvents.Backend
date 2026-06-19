using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Events;

public class VenueRepository : RepositoryBase<Venue>, IVenueRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public VenueRepository(LiveEventsDbContext context, ILogger<VenueRepository> logger)
        : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<Venue> QueryActive()
    {
        return _dbContext.Venues
            .AsNoTracking()
            .Where(v => v.Status);
    }
}
