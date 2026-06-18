using FluentValidation;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Validators;

public sealed class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .Length(6, 100).WithMessage("La contraseña debe tener entre 6 y 100 caracteres.");

        RuleFor(x => x.UserTypeId)
            .NotEmpty().WithMessage("El tipo de usuario es obligatorio.");

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
