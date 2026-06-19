using AutoMapper;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Reservations.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Reservations.Mappings;

public class ReservationProfile : Profile
{
    public ReservationProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.EventTitle, opt => opt.MapFrom(src => src.Event != null ? src.Event.Title : null));

        CreateMap<Reservation, UserReservationListDto>();

        CreateMap<CreateReservationDto, Reservation>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ReservationStatus.PendientePago))
            .ForMember(dest => dest.ReservationCode, opt => opt.Ignore())
            .ForMember(dest => dest.PaidAt, opt => opt.Ignore())
            .ForMember(dest => dest.CancelledAt, opt => opt.Ignore())
            .ForMember(dest => dest.Event, opt => opt.Ignore());
    }
}
