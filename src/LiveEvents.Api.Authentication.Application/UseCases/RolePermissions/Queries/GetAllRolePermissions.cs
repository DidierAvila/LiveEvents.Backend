using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Queries;

public class GetAllRolePermissions
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IMapper _mapper;

    public GetAllRolePermissions(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RolePermissionDto>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _rolePermissionRepository.GetRolePermissionsWithDetailsAsync(cancellationToken);
        return _mapper.Map<IEnumerable<RolePermissionDto>>(entities);
    }
}
