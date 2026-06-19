using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Events;

[Table("reservations", Schema = "events")]
public class Reservation : BaseEntity
{
    public required Guid EventId { get; set; }

    public required int Quantity { get; set; }

    [Required]
    [MaxLength(150)]
    public required string BuyerName { get; set; }

    [Required]
    [MaxLength(255)]
    public required string BuyerEmail { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.PendientePago;

    [MaxLength(20)]
    public string? ReservationCode { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? CancelledAt { get; set; }

    public virtual Event? Event { get; set; }
}
