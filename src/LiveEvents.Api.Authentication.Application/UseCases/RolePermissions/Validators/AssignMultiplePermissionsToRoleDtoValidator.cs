using FluentValidation;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Validators;

public sealed class AssignMultiplePermissionsToRoleDtoValidator : AbstractValidator<AssignMultiplePermissionsToRoleDto>
{
    public AssignMultiplePermissionsToRoleDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("El rol es obligatorio.");

        RuleFor(x => x.PermissionIds)
            .NotNull().WithMessage("La lista de permisos es obligatoria.")
            .Must(ids => ids is { Count: > 0 }).WithMessage("Debe especificar al menos un permiso.");

        RuleForEach(x => x.PermissionIds)
            .NotEmpty().WithMessage("Los permisos no pueden contener identificadores vacíos.");

        RuleFor(x => x.PermissionIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("La lista de permisos no debe contener elementos duplicados.");
    }
}
