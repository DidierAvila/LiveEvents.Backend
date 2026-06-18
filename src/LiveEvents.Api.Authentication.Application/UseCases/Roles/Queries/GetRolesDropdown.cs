using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;

public class GetRolesDropdown
{
    private readonly IRepositoryBase<Role> _roleRepository;
    private readonly IMapper _mapper;

    public GetRolesDropdown(IRepositoryBase<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDropdownDto>> HandleAsync(CancellationToken cancellationToken)
    {
        // Obtener solo roles activos, ordenados alfabéticamente
        var roles = await _roleRepository.GetAll(cancellationToken);
        
        // Filtrar solo roles activos y ordenar por nombre
        var activeRoles = roles
            .Where(r => r.Status == true)
            .OrderBy(r => r.Name)
            .ToList();

        // Map collection of Entities to DTOs using AutoMapper
        return _mapper.Map<IEnumerable<RoleDropdownDto>>(activeRoles);
    }
}
