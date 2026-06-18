using FluentValidation;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Validators;

public sealed class UserFilterDtoValidator : PaginationRequestDtoValidator<UserFilterDto>
{
    public UserFilterDtoValidator()
        : base("name", "email", "createdat", "usertypeid")
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("El rol no es válido.")
            .When(x => x.RoleId.HasValue);

        RuleFor(x => x.UserTypeId)
            .NotEmpty().WithMessage("El tipo de usuario no es válido.")
            .When(x => x.UserTypeId.HasValue);

        RuleFor(x => x)
            .Must(x => !x.CreatedAfter.HasValue || !x.CreatedBefore.HasValue || x.CreatedAfter <= x.CreatedBefore)
            .WithMessage("El rango de fechas no es válido.");
    }
}
