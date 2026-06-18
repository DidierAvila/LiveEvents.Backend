using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Commands;

public class CreateRole
{
    private readonly IRepositoryBase<Role> _roleRepository;
    private readonly IRepositoryBase<Permission> _permissionRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly GetPermissionsForDropdown _getPermissionsForDropdown;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CreateRole(
        IRepositoryBase<Role> roleRepository,
        IRepositoryBase<Permission> permissionRepository,
        IRolePermissionRepository rolePermissionRepository,
        GetPermissionsForDropdown getPermissionsForDropdown,
        IMapper mapper,
        IValidationService validationService)
    {
        _roleRepository = roleRepository;
        _permissionRepository = permissionRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _getPermissionsForDropdown = getPermissionsForDropdown;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<Result<RoleDto>> HandleAsync(CreateRoleDto createRoleDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateAsync(
            createRoleDto,
            "Role.InvalidData",
            "Los datos del rol no son válidos.",
            cancellationToken);
        if (validationResult.IsFailure)
            return Result.Failure<RoleDto>(validationResult.Error);

        // Check if role already exists
        var existingRole = await _roleRepository.Find(x => x.Name == createRoleDto.Name, cancellationToken);
        if (existingRole != null)
            return Result.Failure<RoleDto>(Error.Conflict("Role.NameExists", "Role with this name already exists"));

        // Map DTO to Entity using AutoMapper
        var role = _mapper.Map<Role>(createRoleDto);

        // Create role in repository
        var createdRole = await _roleRepository.Create(role, cancellationToken);

        // Asignar permisos si se proporcionaron
        if (createRoleDto.PermissionIds != null && createRoleDto.PermissionIds.Any())
        {
            await AssignPermissionsToRole(createdRole.Id, createRoleDto.PermissionIds, cancellationToken);
        }

        // Map Entity to DTO using AutoMapper
        var roleDto = _mapper.Map<RoleDto>(createdRole);

        // Cargar permisos asignados para incluir en la respuesta
        if (createRoleDto.PermissionIds != null && createRoleDto.PermissionIds.Any())
        {
            roleDto.Permissions = (List<PermissionDropdownDto>?)await _getPermissionsForDropdown.HandleAsync(roleDto.Id, cancellationToken);
        }

        return Result.Success(roleDto);
    }

    /// <summary>
    /// Asigna m�ltiples permisos a un rol reci�n creado
    /// </summary>
    private async Task AssignPermissionsToRole(Guid roleId, List<Guid> permissionIds, CancellationToken cancellationToken)
    {
        // Validar que todos los permisos existen
        var validPermissions = new List<Permission>();
        foreach (var permissionId in permissionIds)
        {
            var permission = await _permissionRepository.Find(p => p.Id == permissionId, cancellationToken);
            if (permission != null)
            {
                validPermissions.Add(permission);

                // Crear la relaci�n RolePermission
                var rolePermission = new RolePermission
                {
                    RoleId = roleId,
                    PermissionId = permissionId
                };

                await _rolePermissionRepository.Create(rolePermission, cancellationToken);
            }
        }
    }
}
