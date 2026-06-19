using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Domain.Ports.Events;

public interface IVenueRepository : IRepositoryBase<Venue>
{
    IQueryable<Venue> QueryActive();
}
