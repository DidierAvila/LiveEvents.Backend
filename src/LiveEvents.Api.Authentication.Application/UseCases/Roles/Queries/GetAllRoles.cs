using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;

public class GetAllRoles
{
    private readonly IRepositoryBase<Role> _roleRepository;
    private readonly IMapper _mapper;

    public GetAllRoles(IRepositoryBase<Role> roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoleDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAll(cancellationToken);

        // Map Entities to DTOs using AutoMapper
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }
}
