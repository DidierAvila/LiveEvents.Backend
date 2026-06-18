using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;

public class GetPermissionById
{
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public GetPermissionById(IRepositoryBase<Permission> permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<PermissionDto?> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByID(id, cancellationToken);
        if (permission == null)
            return null;

        // Map Entity to DTO using AutoMapper
        return _mapper.Map<PermissionDto>(permission);
    }
}
