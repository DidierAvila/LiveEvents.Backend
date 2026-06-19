using FluentValidation;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Validators;

public sealed class CreateEventDtoValidator : AbstractValidator<CreateEventDto>
{
    public CreateEventDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El titulo es obligatorio.")
            .Length(5, 100).WithMessage("El titulo debe tener entre 5 y 100 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripcion es obligatoria.")
            .Length(10, 500).WithMessage("La descripcion debe tener entre 10 y 500 caracteres.");

        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("El venue es obligatorio.");

        RuleFor(x => x.MaxCapacity)
            .GreaterThan(0).WithMessage("La capacidad maxima debe ser un entero positivo.");

        RuleFor(x => x.StartsAt)
            .Must(date => date > DateTime.Now)
            .WithMessage("La fecha y hora de inicio debe ser futura.");

        RuleFor(x => x.EndsAt)
            .GreaterThan(x => x.StartsAt)
            .WithMessage("La fecha y hora de fin debe ser posterior al inicio.");

        RuleFor(x => x.TicketPrice)
            .GreaterThan(0).WithMessage("El precio de entrada debe ser positivo.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de evento no es valido.")
            .Must(type => Enum.IsDefined(type) && type != 0)
            .WithMessage("El tipo de evento es obligatorio.");
    }
}

public sealed class EventFilterDtoValidator : PaginationRequestDtoValidator<EventFilterDto>
{
    public EventFilterDtoValidator()
        : base("title", "startsat", "endsat", "ticketprice", "maxcapacity", "createdat", "venuename")
    {
        RuleFor(x => x.VenueId)
            .NotEmpty().WithMessage("El venue no es valido.")
            .When(x => x.VenueId.HasValue);

        RuleFor(x => x)
            .Must(x => !x.StartsFrom.HasValue || !x.StartsTo.HasValue || x.StartsFrom <= x.StartsTo)
            .WithMessage("El rango de fechas de inicio no es valido.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("El tipo de evento no es valido.")
            .When(x => x.Type.HasValue);

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del evento no es valido.")
            .When(x => x.Status.HasValue);
    }
}
