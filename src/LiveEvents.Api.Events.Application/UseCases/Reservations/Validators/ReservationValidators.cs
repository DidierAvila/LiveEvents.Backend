using FluentValidation;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Validators;

public sealed class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("El evento es obligatorio.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(1).WithMessage("La cantidad de entradas debe ser 1 o mas.");

        RuleFor(x => x.BuyerName)
            .NotEmpty().WithMessage("El nombre del comprador es obligatorio.")
            .MaximumLength(150).WithMessage("El nombre del comprador no puede superar los 150 caracteres.");

        RuleFor(x => x.BuyerEmail)
            .NotEmpty().WithMessage("El email del comprador es obligatorio.")
            .EmailAddress().WithMessage("El email del comprador no tiene un formato valido.")
            .MaximumLength(255).WithMessage("El email del comprador no puede superar los 255 caracteres.");
    }
}

public sealed class UserReservationFilterDtoValidator : PaginationRequestDtoValidator<UserReservationFilterDto>
{
    public UserReservationFilterDtoValidator()
        : base("createdat", "status", "quantity", "reservationcode", "eventtitle", "eventstartsat")
    {
        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("El evento no es valido.")
            .When(x => x.EventId.HasValue);

        RuleFor(x => x)
            .Must(x => !x.CreatedFrom.HasValue || !x.CreatedTo.HasValue || x.CreatedFrom <= x.CreatedTo)
            .WithMessage("El rango de fechas de creacion no es valido.");
    }
}
