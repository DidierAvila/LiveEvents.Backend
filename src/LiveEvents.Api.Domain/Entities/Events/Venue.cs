using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Events;

[Table("venues", Schema = "events")]
public class Venue : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public required string Name { get; set; }

    public required int Capacity { get; set; }

    [Required]
    [MaxLength(100)]
    public required string City { get; set; }

    public bool Status { get; set; } = true;

    public virtual ICollection<Event> Events { get; set; } = [];
}
