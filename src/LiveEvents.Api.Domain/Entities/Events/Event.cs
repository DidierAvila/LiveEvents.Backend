using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Events;

[Table("events", Schema = "events")]
public class Event : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(500)]
    public required string Description { get; set; }

    public required Guid VenueId { get; set; }

    public required int MaxCapacity { get; set; }

    public required DateTime StartsAt { get; set; }

    public required DateTime EndsAt { get; set; }

    [Column(TypeName = "numeric(10,2)")]
    public required decimal TicketPrice { get; set; }

    public EventType Type { get; set; }

    public EventStatus Status { get; set; } = EventStatus.Activo;

    public virtual Venue? Venue { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = [];
}
