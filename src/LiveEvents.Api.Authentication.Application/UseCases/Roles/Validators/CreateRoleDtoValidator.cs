using FluentValidation;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Validators;

public sealed class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre del rol es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del rol no puede superar los 100 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.")
            .When(x => x.Description is not null);

        RuleForEach(x => x.PermissionIds!)
            .NotEmpty().WithMessage("Los permisos no pueden contener identificadores vacíos.")
            .When(x => x.PermissionIds is not null);

        RuleFor(x => x.PermissionIds)
            .Must(HaveUniqueIds)
            .WithMessage("La lista de permisos no debe contener elementos duplicados.")
            .When(x => x.PermissionIds is not null);
    }

    private static bool HaveUniqueIds(IEnumerable<Guid>? ids)
        => ids is null || ids.Distinct().Count() == ids.Count();
}
