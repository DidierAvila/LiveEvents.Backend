using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Events;

public class ReservationRepository : RepositoryBase<Reservation>, IReservationRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public ReservationRepository(LiveEventsDbContext context, ILogger<ReservationRepository> logger)
        : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<Reservation> QueryWithEvent()
    {
        return _dbContext.Reservations
            .AsNoTracking()
            .Include(r => r.Event)
            .ThenInclude(e => e!.Venue);
    }

    public Task<int> GetUnavailableSeatsForEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return _dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.EventId == eventId &&
                (r.Status == ReservationStatus.PendientePago ||
                 r.Status == ReservationStatus.Confirmada ||
                 r.Status == ReservationStatus.Perdida))
            .SumAsync(r => r.Quantity, cancellationToken);
    }

    public Task<int> GetConfirmedSeatsForEventAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return _dbContext.Reservations
            .AsNoTracking()
            .Where(r => r.EventId == eventId && r.Status == ReservationStatus.Confirmada)
            .SumAsync(r => r.Quantity, cancellationToken);
    }

    public Task<bool> ReservationCodeExistsAsync(string reservationCode, CancellationToken cancellationToken)
    {
        return _dbContext.Reservations
            .AsNoTracking()
            .AnyAsync(r => r.ReservationCode == reservationCode, cancellationToken);
    }
}
