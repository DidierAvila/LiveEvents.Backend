using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Dtos;

public class CreateEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid VenueId { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal TicketPrice { get; set; }
    public EventType Type { get; set; }
}

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid VenueId { get; set; }
    public string? VenueName { get; set; }
    public string? VenueCity { get; set; }
    public int MaxCapacity { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal TicketPrice { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class EventListDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid VenueId { get; set; }
    public string? VenueName { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
    public decimal TicketPrice { get; set; }
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public int MaxCapacity { get; set; }
}

public class EventDropdownDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class EventFilterDto : PaginationRequestDto
{
    public string? Search { get; set; }
    public EventType? Type { get; set; }
    public DateTime? StartsFrom { get; set; }
    public DateTime? StartsTo { get; set; }
    public Guid? VenueId { get; set; }
    public EventStatus? Status { get; set; }
}

public class OccupationReportDto
{
    public Guid EventId { get; set; }
    public string EventTitle { get; set; } = string.Empty;
    public int ConfirmedTickets { get; set; }
    public int AvailableTickets { get; set; }
    public decimal OccupancyPercentage { get; set; }
    public decimal TotalRevenue { get; set; }
    public EventStatus Status { get; set; }
}
