using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Validators;

public sealed class PermissionFilterDtoValidator : PaginationRequestDtoValidator<PermissionFilterDto>
{
    public PermissionFilterDtoValidator()
        : base("name", "description", "status", "createdat")
    {
    }
}
