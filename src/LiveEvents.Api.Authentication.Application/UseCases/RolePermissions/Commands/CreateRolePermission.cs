using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Dtos;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;

public class CreateRolePermission
{
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IMapper _mapper;

    public CreateRolePermission(IRolePermissionRepository rolePermissionRepository, IMapper mapper)
    {
        _rolePermissionRepository = rolePermissionRepository;
        _mapper = mapper;
    }

    public async Task<Result<RolePermissionDto>> HandleAsync(CreateRolePermissionDto request, CancellationToken cancellationToken = default)
    {
        // Verificar si ya existe la asignación
        var exists = await _rolePermissionRepository.ExistsAsync(request.RoleId, request.PermissionId, cancellationToken);
        if (exists)
        {
            return Result.Failure<RolePermissionDto>(Error.Conflict("RolePermission.AlreadyExists", "El permiso ya está asignado a este rol"));
        }

        var entity = _mapper.Map<RolePermission>(request);

        var createdEntity = await _rolePermissionRepository.Create(entity, cancellationToken);
        return Result.Success(_mapper.Map<RolePermissionDto>(createdEntity));
    }
}
