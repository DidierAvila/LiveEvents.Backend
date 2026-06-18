using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Mappings;

public class RolePermissionProfile : Profile
{
    public RolePermissionProfile()
    {
        // Entity to DTO
        CreateMap<RolePermission, RolePermissionDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.PermissionName, opt => opt.MapFrom(src => src.Permission.Name));

        // DTO to Entity
        CreateMap<CreateRolePermissionDto, RolePermission>()
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Permission, opt => opt.Ignore());

        CreateMap<AssignPermissionToRoleDto, RolePermission>()
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Permission, opt => opt.Ignore());
    }
}
