using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;
public class GetActivePermissions
{
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public GetActivePermissions(IRepositoryBase<Permission> permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PermissionDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var activePermissions = await _permissionRepository.Finds(x => x.Status == true, cancellationToken);
        return _mapper.Map<IEnumerable<PermissionDto>>(activePermissions);
    }
}
