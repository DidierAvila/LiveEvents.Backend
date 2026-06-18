using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Mappings;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        // AutoMapper mapea automáticamente propiedades con nombres iguales
        CreateMap<Role, RoleDto>();
        CreateMap<Permission, PermissionDto>();
        CreateMap<Session, SessionDto>();

        // Mapeo personalizado para UserRole -> UserRoleDto
        // Mapea usando la navigation property Role
        CreateMap<UserRole, UserRoleDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Role.Description))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Role.Status));

        // Mapeo personalizado para UserRole -> RoleDropdownDto (optimizado para GetById)
        // Solo mapea Id y Name para mejor rendimiento
        CreateMap<UserRole, RoleDropdownDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name));
    }
}
