using AutoMapper;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Events.Helpers;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;

public class CreateReservation
{
    private readonly IEventRepository _eventRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CreateReservation(
        IEventRepository eventRepository,
        IReservationRepository reservationRepository,
        IMapper mapper,
        IValidationService validationService)
    {
        _eventRepository = eventRepository;
        _reservationRepository = reservationRepository;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<Result<ReservationDto>> HandleAsync(CreateReservationDto createReservationDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateAsync(
            createReservationDto,
            "Reservation.InvalidData",
            "Los datos de la reserva no son validos.",
            cancellationToken);
        if (validationResult.IsFailure)
        {
            return Result.Failure<ReservationDto>(validationResult.Error);
        }

        var eventEntity = await _eventRepository.GetByID(createReservationDto.EventId, cancellationToken);
        if (eventEntity is null)
        {
            return Result.Failure<ReservationDto>(Error.NotFound("Event.NotFound", "El evento indicado no existe."));
        }

        var currentStatus = EventStatusResolver.GetCurrentStatus(eventEntity);
        if (currentStatus != EventStatus.Activo)
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.EventUnavailable", "Solo se permiten reservas para eventos activos."));
        }

        var now = DateTime.Now;
        if (eventEntity.StartsAt <= now.AddHours(1))
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.TooLate", "No se permiten reservas para eventos que inicien en menos de 1 hora."));
        }

        if (eventEntity.StartsAt <= now.AddHours(24) && createReservationDto.Quantity > 5)
        {
            return Result.Failure<ReservationDto>(Error.Validation("Reservation.MaxFiveEntries", "Cuando faltan menos de 24 horas para el evento solo se permiten hasta 5 entradas por transaccion."));
        }

        if (eventEntity.TicketPrice > 100 && createReservationDto.Quantity > 10)
        {
            return Result.Failure<ReservationDto>(Error.Validation("Reservation.MaxTenEntries", "Los eventos con precio mayor a $100 permiten maximo 10 entradas por transaccion."));
        }

        var unavailableSeats = await _reservationRepository.GetUnavailableSeatsForEventAsync(eventEntity.Id, cancellationToken);
        if (unavailableSeats + createReservationDto.Quantity > eventEntity.MaxCapacity)
        {
            return Result.Failure<ReservationDto>(Error.Conflict("Reservation.NoAvailability", "La cantidad solicitada excede las entradas disponibles."));
        }

        var reservation = _mapper.Map<Reservation>(createReservationDto);

        var createdReservation = await _reservationRepository.Create(reservation, cancellationToken);

        var dto = _mapper.Map<ReservationDto>(createdReservation);
        dto.EventTitle = eventEntity.Title;
        return Result.Success(dto);
    }
}
