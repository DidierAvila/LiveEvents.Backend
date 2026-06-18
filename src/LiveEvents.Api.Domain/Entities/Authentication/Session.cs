using System.ComponentModel.DataAnnotations.Schema;

namespace LiveEvents.Api.Domain.Entities.Authentication;

[Table(name: "sessions", Schema = "authentication")]
public partial class Session
{
    public Guid Id { get; set; }
    public string SessionToken { get; set; } = null!;
    public Guid? UserId { get; set; }
    public DateTime Expires { get; set; }
    public virtual User? User { get; set; }
}
