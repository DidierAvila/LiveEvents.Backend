using AutoMapper;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;
using LiveEvents.Api.Infrastructure.Adapters.Pagination;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Queries;

public sealed class UserReservationPaginationService : PaginationServiceBase<Reservation, UserReservationListDto, UserReservationFilterDto>
{
    private readonly IMapper _mapper;

    public UserReservationPaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected override IQueryable<Reservation> ApplyFilters(IQueryable<Reservation> query, UserReservationFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(r =>
                r.BuyerName.ToLower().Contains(search) ||
                r.BuyerEmail.ToLower().Contains(search) ||
                (r.Event != null && r.Event.Title.ToLower().Contains(search)) ||
                (r.ReservationCode != null && r.ReservationCode.ToLower().Contains(search)));
        }

        if (filter.EventId.HasValue)
        {
            query = query.Where(r => r.EventId == filter.EventId.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.Status == filter.Status.Value);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(r => r.CreatedAt >= filter.CreatedFrom.Value);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(r => r.CreatedAt <= filter.CreatedTo.Value);
        }

        return query;
    }

    protected override IQueryable<Reservation> ApplySorting(IQueryable<Reservation> query, string? sortBy, bool sortDescending)
    {
        return SortingHelper.CreateSortingBuilder(query)
            .AddSortMapping("createdat", r => r.CreatedAt)
            .AddSortMapping("status", r => r.Status)
            .AddSortMapping("quantity", r => r.Quantity)
            .AddSortMapping("reservationcode", r => r.ReservationCode ?? string.Empty)
            .AddSortMapping("eventtitle", r => r.Event != null ? r.Event.Title : string.Empty)
            .AddSortMapping("eventstartsat", r => r.Event != null ? r.Event.StartsAt : DateTime.MinValue)
            .SetDefaultSort(r => r.CreatedAt)
            .ApplySorting(sortBy, sortDescending);
    }

    protected override Task<IEnumerable<UserReservationListDto>> MapToDto(IEnumerable<Reservation> entities, CancellationToken cancellationToken)
    {
        var source = entities.ToList();
        var dtos = _mapper.Map<List<UserReservationListDto>>(source);

        for (var index = 0; index < source.Count; index++)
        {
            if (source[index].Event is not null)
            {
                dtos[index].EventTitle = source[index].Event!.Title;
                dtos[index].EventStartsAt = source[index].Event!.StartsAt;
                dtos[index].VenueName = source[index].Event!.Venue?.Name;
            }
        }

        return Task.FromResult<IEnumerable<UserReservationListDto>>(dtos);
    }
}
