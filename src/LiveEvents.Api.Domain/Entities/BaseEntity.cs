namespace LiveEvents.Api.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
