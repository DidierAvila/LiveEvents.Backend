using FluentValidation;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Commands;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Handlers;
using LiveEvents.Api.Events.Application.UseCases.Events.Mappings;
using LiveEvents.Api.Events.Application.UseCases.Events.Queries;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Handlers;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Mappings;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Queries;
using LiveEvents.Api.Events.Application.UseCases.Venues.Handlers;
using LiveEvents.Api.Events.Application.UseCases.Venues.Queries;
using LiveEvents.Api.Events.Application.Validation;
using LiveEvents.Api.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveEvents.Api.Events.Application;

public static class EventsApplicationDependencyInjection
{
    public static IServiceCollection AddEventsApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<EventProfile>();
            cfg.AddProfile<ReservationProfile>();
        });

        services.AddValidatorsFromAssemblyContaining<ValidationService>();
        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<CreateEvent>();
        services.AddScoped<GetEventById>();
        services.AddScoped<GetAllEventsFiltered>();
        services.AddScoped<GetOccupationReport>();
        services.AddScoped<GetEventsForDropdown>();
        services.AddScoped<IPaginationServiceBase<Event, EventListDto, EventFilterDto>, EventPaginationService>();
        services.AddScoped<GetMyReservationsFiltered>();
        services.AddScoped<IPaginationServiceBase<Reservation, UserReservationListDto, UserReservationFilterDto>, UserReservationPaginationService>();
        services.AddScoped<IEventCommandHandler, EventCommandHandler>();
        services.AddScoped<IEventQueryHandler, EventQueryHandler>();

        services.AddScoped<CreateReservation>();
        services.AddScoped<ConfirmReservationPayment>();
        services.AddScoped<CancelReservation>();
        services.AddScoped<IReservationCommandHandler, ReservationCommandHandler>();
        services.AddScoped<IReservationQueryHandler, ReservationQueryHandler>();

        services.AddScoped<GetVenuesForDropdown>();
        services.AddScoped<IVenueQueryHandler, VenueQueryHandler>();

        services.AddInfrastructure(configuration);

        return services;
    }
}
