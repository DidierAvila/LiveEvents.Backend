using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Authentication;

[Table(name: "users", Schema = "authentication")]
public partial class User : BaseEntity
{
    public required string Name { get; set; }
    public string? Address { get; set; }
    public required string Email { get; set; }
    public string? Password { get; set; }
    public string? Image { get; set; }
    public string? Phone { get; set; }
    public required Guid UserTypeId { get; set; }
    public required string ExtraData { get; set; }
    public UserStatus Status { get; set; }
    public virtual ICollection<Session> Sessions { get; set; } = [];
    public virtual ICollection<Role> Roles { get; set; } = [];
    public virtual UserType? UserType { get; set; }
}
