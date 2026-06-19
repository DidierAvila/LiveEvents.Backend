using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

namespace LiveEvents.Api.Events.Application.Tests.Support;

internal static class EventsTestData
{
    public static CreateEventDto BuildCreateEventDto(Action<CreateEventDto>? configure = null)
    {
        var dto = new CreateEventDto
        {
            Title = "Evento de arquitectura",
            Description = "Sesion para revisar reglas de negocio y patrones de backend.",
            VenueId = Guid.NewGuid(),
            MaxCapacity = 80,
            StartsAt = DateTime.Now.AddDays(5).Date.AddHours(18),
            EndsAt = DateTime.Now.AddDays(5).Date.AddHours(20),
            TicketPrice = 90,
            Type = EventType.Conferencia
        };

        configure?.Invoke(dto);
        return dto;
    }

    public static Venue BuildVenue(Action<Venue>? configure = null)
    {
        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            Name = "Auditorio Central",
            Capacity = 100,
            City = "Bogota",
            Status = true,
            CreatedAt = DateTime.Now
        };

        configure?.Invoke(venue);
        return venue;
    }

    public static Event BuildEvent(Action<Event>? configure = null)
    {
        var eventEntity = new Event
        {
            Id = Guid.NewGuid(),
            Title = "Concierto premium",
            Description = "Evento de prueba para reservas y estados.",
            VenueId = Guid.NewGuid(),
            MaxCapacity = 200,
            StartsAt = DateTime.Now.AddDays(3),
            EndsAt = DateTime.Now.AddDays(3).AddHours(2),
            TicketPrice = 80,
            Type = EventType.Concierto,
            Status = EventStatus.Activo,
            CreatedAt = DateTime.Now
        };

        configure?.Invoke(eventEntity);
        return eventEntity;
    }

    public static CreateReservationDto BuildCreateReservationDto(Action<CreateReservationDto>? configure = null)
    {
        var dto = new CreateReservationDto
        {
            EventId = Guid.NewGuid(),
            Quantity = 2,
            BuyerName = "Alejo Perez",
            BuyerEmail = "alejo@example.com"
        };

        configure?.Invoke(dto);
        return dto;
    }

    public static Reservation BuildReservation(Action<Reservation>? configure = null)
    {
        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            EventId = Guid.NewGuid(),
            Quantity = 2,
            BuyerName = "Alejo Perez",
            BuyerEmail = "alejo@example.com",
            Status = ReservationStatus.Confirmada,
            ReservationCode = "EV-000001",
            CreatedAt = DateTime.Now.AddDays(-1)
        };

        configure?.Invoke(reservation);
        return reservation;
    }

    public static DateTime NextSaturdayAt(int hour, int minute)
    {
        var date = DateTime.Today;
        while (date.DayOfWeek != DayOfWeek.Saturday)
        {
            date = date.AddDays(1);
        }

        return date.AddHours(hour).AddMinutes(minute);
    }
}
