using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Validators;

public sealed class RoleFilterDtoValidator : PaginationRequestDtoValidator<RoleFilterDto>
{
    public RoleFilterDtoValidator()
        : base("name", "description", "status", "createdat")
    {
    }
}
