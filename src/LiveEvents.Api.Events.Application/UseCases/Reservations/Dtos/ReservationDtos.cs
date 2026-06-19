using LiveEvents.Api.Domain.Entities.Events;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

public class CreateReservationDto
{
    public Guid EventId { get; set; }
    public int Quantity { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
}

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string? EventTitle { get; set; }
    public int Quantity { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public string BuyerEmail { get; set; } = string.Empty;
    public ReservationStatus Status { get; set; }
    public string? ReservationCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}

public class UserReservationListDto
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public string? EventTitle { get; set; }
    public DateTime? EventStartsAt { get; set; }
    public string? VenueName { get; set; }
    public int Quantity { get; set; }
    public ReservationStatus Status { get; set; }
    public string? ReservationCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? CancelledAt { get; set; }
}

public class UserReservationFilterDto : LiveEvents.Api.Common.Features.Pagination.Dtos.PaginationRequestDto
{
    public string? Search { get; set; }
    public Guid? EventId { get; set; }
    public ReservationStatus? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}
