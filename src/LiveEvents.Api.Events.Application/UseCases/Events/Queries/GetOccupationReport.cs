using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Queries;

public class GetOccupationReport
{
    private readonly IEventRepository _eventRepository;
    private readonly IReservationRepository _reservationRepository;

    public GetOccupationReport(IEventRepository eventRepository, IReservationRepository reservationRepository)
    {
        _eventRepository = eventRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<OccupationReportDto?> HandleAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.QueryWithDetails()
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
        {
            return null;
        }

        var confirmedTickets = await _reservationRepository.GetConfirmedSeatsForEventAsync(eventId, cancellationToken);
        var unavailableTickets = await _reservationRepository.GetUnavailableSeatsForEventAsync(eventId, cancellationToken);
        var availableTickets = Math.Max(0, eventEntity.MaxCapacity - unavailableTickets);

        return new OccupationReportDto
        {
            EventId = eventEntity.Id,
            EventTitle = eventEntity.Title,
            ConfirmedTickets = confirmedTickets,
            AvailableTickets = availableTickets,
            OccupancyPercentage = eventEntity.MaxCapacity == 0
                ? 0
                : Math.Round((decimal)confirmedTickets / eventEntity.MaxCapacity * 100, 2),
            TotalRevenue = Math.Round(eventEntity.TicketPrice * confirmedTickets, 2),
            Status = EventStatusResolver.GetCurrentStatus(eventEntity)
        };
    }
}
