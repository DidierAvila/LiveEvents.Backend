using AutoMapper;
using LiveEvents.Api.Domain.Entities.Events;
using LiveEvents.Api.Events.Application.UseCases.Events.Dtos;

namespace LiveEvents.Api.Events.Application.UseCases.Events.Mappings;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventDto>()
            .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue != null ? src.Venue.Name : null))
            .ForMember(dest => dest.VenueCity, opt => opt.MapFrom(src => src.Venue != null ? src.Venue.City : null));

        CreateMap<Event, EventListDto>()
            .ForMember(dest => dest.VenueName, opt => opt.MapFrom(src => src.Venue != null ? src.Venue.Name : null));

        CreateMap<CreateEventDto, Event>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => EventStatus.Activo))
            .ForMember(dest => dest.Venue, opt => opt.Ignore())
            .ForMember(dest => dest.Reservations, opt => opt.Ignore());
    }
}
