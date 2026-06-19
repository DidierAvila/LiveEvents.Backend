using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Commands;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Queries;
using LiveEvents.Api.Events.Application.Validation;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Handlers;

public interface IReservationCommandHandler
{
    Task<Result<ReservationDto>> CreateReservation(CreateReservationDto command, CancellationToken cancellationToken);
    Task<Result<ReservationDto>> ConfirmReservationPayment(Guid reservationId, CancellationToken cancellationToken);
    Task<Result<ReservationDto>> CancelReservation(Guid reservationId, CancellationToken cancellationToken);
}

public interface IReservationQueryHandler
{
    Task<Result<PaginationResponseDto<UserReservationListDto>>> GetMyReservations(
        string buyerEmail,
        UserReservationFilterDto filter,
        CancellationToken cancellationToken);
}

public class ReservationCommandHandler : IReservationCommandHandler
{
    private readonly CreateReservation _createReservation;
    private readonly ConfirmReservationPayment _confirmReservationPayment;
    private readonly CancelReservation _cancelReservation;

    public ReservationCommandHandler(
        CreateReservation createReservation,
        ConfirmReservationPayment confirmReservationPayment,
        CancelReservation cancelReservation)
    {
        _createReservation = createReservation;
        _confirmReservationPayment = confirmReservationPayment;
        _cancelReservation = cancelReservation;
    }

    public Task<Result<ReservationDto>> CreateReservation(CreateReservationDto command, CancellationToken cancellationToken)
    {
        return _createReservation.HandleAsync(command, cancellationToken);
    }

    public Task<Result<ReservationDto>> ConfirmReservationPayment(Guid reservationId, CancellationToken cancellationToken)
    {
        return _confirmReservationPayment.HandleAsync(reservationId, cancellationToken);
    }

    public Task<Result<ReservationDto>> CancelReservation(Guid reservationId, CancellationToken cancellationToken)
    {
        return _cancelReservation.HandleAsync(reservationId, cancellationToken);
    }
}

public class ReservationQueryHandler : IReservationQueryHandler
{
    private readonly GetMyReservationsFiltered _getMyReservationsFiltered;
    private readonly IValidationService _validationService;

    public ReservationQueryHandler(
        GetMyReservationsFiltered getMyReservationsFiltered,
        IValidationService validationService)
    {
        _getMyReservationsFiltered = getMyReservationsFiltered;
        _validationService = validationService;
    }

    public async Task<Result<PaginationResponseDto<UserReservationListDto>>> GetMyReservations(
        string buyerEmail,
        UserReservationFilterDto filter,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(
                filter,
                "Reservation.InvalidFilter",
                "Los filtros de reservas no son validos.",
                cancellationToken);

            if (validationResult.IsFailure)
            {
                return Result.Failure<PaginationResponseDto<UserReservationListDto>>(validationResult.Error);
            }

            var reservations = await _getMyReservationsFiltered.HandleAsync(buyerEmail, filter, cancellationToken);
            return Result.Success(reservations);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PaginationResponseDto<UserReservationListDto>>(Error.Validation("Reservation.InvalidFilter", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginationResponseDto<UserReservationListDto>>(Error.Failure("Reservation.GetMine", ex.Message));
        }
    }
}
