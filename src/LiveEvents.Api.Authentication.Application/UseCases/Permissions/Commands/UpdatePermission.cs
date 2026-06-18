using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Commands;

public class UpdatePermission
{
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IMapper _mapper;

    public UpdatePermission(IRepositoryBase<Permission> permissionRepository, IMapper mapper)
    {
        _permissionRepository = permissionRepository;
        _mapper = mapper;
    }

    public async Task<Result<PermissionDto>> HandleAsync(Guid id, UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken)
    {
        // Find existing permission
        var permission = await _permissionRepository.Find(x => x.Id == id, cancellationToken);
        if (permission == null)
            return Result.Failure<PermissionDto>(Error.NotFound("Permission.NotFound", "Permission not found"));

        // Validate that the name doesn't already exist (if it's being updated)
        if (!string.IsNullOrWhiteSpace(updatePermissionDto.Name) &&
            updatePermissionDto.Name != permission.Name)
        {
            var existingPermission = await _permissionRepository.Find(x => x.Name == updatePermissionDto.Name, cancellationToken);
            if (existingPermission != null)
                return Result.Failure<PermissionDto>(Error.Conflict("Permission.NameExists", "A Permission with this name already exists"));
        }

        // Map updated values from DTO to existing entity
        _mapper.Map(updatePermissionDto, permission);

        // Update the Permission
        await _permissionRepository.Update(permission, cancellationToken);

        // Map Entity back to DTO for return
        return Result.Success(_mapper.Map<PermissionDto>(permission));
    }
}
