using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Queries;

public class GetRolesByPermission
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IMapper _mapper;

    public GetRolesByPermission(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RolePermissionDto>> HandleAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        var entities = await _rolePermissionRepository.GetRolesByPermissionIdAsync(permissionId, cancellationToken);
        return _mapper.Map<IEnumerable<RolePermissionDto>>(entities);
    }
}
