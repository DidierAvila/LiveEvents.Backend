using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Domain.Ports.Events;

public interface IReservationRepository : IRepositoryBase<Reservation>
{
    IQueryable<Reservation> QueryWithEvent();
    Task<int> GetUnavailableSeatsForEventAsync(Guid eventId, CancellationToken cancellationToken);
    Task<int> GetConfirmedSeatsForEventAsync(Guid eventId, CancellationToken cancellationToken);
    Task<bool> ReservationCodeExistsAsync(string reservationCode, CancellationToken cancellationToken);
}
