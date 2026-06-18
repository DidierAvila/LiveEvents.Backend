using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Queries;

public class GetPermissionsByRole
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IMapper _mapper;

    public GetPermissionsByRole(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RolePermissionDto>> HandleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var entities = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId, cancellationToken);
        return _mapper.Map<IEnumerable<RolePermissionDto>>(entities);
    }
}
