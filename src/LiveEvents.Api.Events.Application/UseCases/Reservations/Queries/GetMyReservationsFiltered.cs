using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Domain.Ports.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Queries;

public class GetMyReservationsFiltered
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPaginationServiceBase<Reservation, UserReservationListDto, UserReservationFilterDto> _paginationService;

    public GetMyReservationsFiltered(
        IReservationRepository reservationRepository,
        IPaginationServiceBase<Reservation, UserReservationListDto, UserReservationFilterDto> paginationService)
    {
        _reservationRepository = reservationRepository;
        _paginationService = paginationService;
    }

    public Task<PaginationResponseDto<UserReservationListDto>> HandleAsync(string buyerEmail, UserReservationFilterDto filter, CancellationToken cancellationToken)
    {
        var normalizedEmail = buyerEmail.Trim().ToLower();

        return _paginationService.GetPaginatedWithExpressionAsync(
            _reservationRepository.QueryWithEvent(),
            filter,
            reservation => reservation.BuyerEmail.ToLower() == normalizedEmail,
            cancellationToken);
    }
}
