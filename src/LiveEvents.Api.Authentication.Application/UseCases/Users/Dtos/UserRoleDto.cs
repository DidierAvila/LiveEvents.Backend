namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;

public class UserRoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Status { get; set; }
}
