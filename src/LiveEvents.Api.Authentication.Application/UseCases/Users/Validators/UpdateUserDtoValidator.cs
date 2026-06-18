using FluentValidation;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Validators;

public sealed class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo no puede estar vacío.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");
        });

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre no puede estar vacío.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");
        });

        When(x => x.UserTypeId.HasValue, () =>
        {
            RuleFor(x => x.UserTypeId)
                .NotEmpty().WithMessage("El tipo de usuario no es válido.");
        });

        RuleForEach(x => x.RoleIds!)
            .NotEmpty().WithMessage("Los roles no pueden contener identificadores vacíos.")
            .When(x => x.RoleIds is not null);

        RuleFor(x => x.RoleIds)
            .Must(HaveUniqueIds)
            .WithMessage("La lista de roles no debe contener elementos duplicados.")
            .When(x => x.RoleIds is not null);
    }

    private static bool HaveUniqueIds(IEnumerable<Guid>? ids)
        => ids is null || ids.Distinct().Count() == ids.Count();
}
