using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;

public class GetAllPermissions
{
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public GetAllPermissions(IRepositoryBase<Permission> permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var permissions = await _permissionRepository.GetAll(cancellationToken);
        return _mapper.Map<IEnumerable<PermissionDto>>(permissions);
    }
}
