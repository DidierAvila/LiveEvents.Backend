using FluentValidation;
using LiveEvents.Api.Common.Features.Pagination.Dtos;

namespace LiveEvents.Api.Events.Application.Validation;

public abstract class PaginationRequestDtoValidator<T> : AbstractValidator<T>
    where T : PaginationRequestDto
{
    protected PaginationRequestDtoValidator(params string[] allowedSortFields)
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("La pagina debe ser mayor que 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamano de pagina debe estar entre 1 y 100.");

        if (allowedSortFields.Length == 0)
        {
            return;
        }

        var allowedFields = new HashSet<string>(allowedSortFields, StringComparer.OrdinalIgnoreCase);
        var allowedFieldsMessage = string.Join(", ", allowedSortFields);

        RuleFor(x => x.SortBy)
            .Must(sortBy => string.IsNullOrWhiteSpace(sortBy) || allowedFields.Contains(sortBy))
            .WithMessage($"El campo de ordenamiento no es valido. Valores permitidos: {allowedFieldsMessage}.");
    }
}
