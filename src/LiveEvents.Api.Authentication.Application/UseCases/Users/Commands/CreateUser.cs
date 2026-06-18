using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;
using BC = BCrypt.Net.BCrypt;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;

public class CreateUser
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IRepositoryBase<Role> _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public CreateUser(
        IRepositoryBase<User> userRepository,
        IRepositoryBase<Role> roleRepository,
        IUserRoleRepository userRoleRepository,
        IMapper mapper,
        IValidationService validationService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userRoleRepository = userRoleRepository;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<Result<UserDto>> HandleAsync(CreateUserDto createUserDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateAsync(
            createUserDto,
            "User.InvalidData",
            "Los datos del usuario no son válidos.",
            cancellationToken);
        if (validationResult.IsFailure)
            return Result.Failure<UserDto>(validationResult.Error);

        // Check if user already exists
        var existingUser = await _userRepository.Find(x => x.Email == createUserDto.Email, cancellationToken);
        if (existingUser != null)
            return Result.Failure<UserDto>(Error.Conflict("User.EmailExists", "User with this email already exists"));

        // Map DTO to Entity using AutoMapper
        var user = _mapper.Map<User>(createUserDto);

        // Encrypt password before saving
        if (!string.IsNullOrEmpty(user.Password))
        {
            user.Password = BC.HashPassword(user.Password, 12);
        }

        // Create user in repository
        var createdUser = await _userRepository.Create(user, cancellationToken);

        // Asignar roles si se proporcionaron
        if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
        {
            await AssignRolesToUser(createdUser.Id, createUserDto.RoleIds, cancellationToken);
        }

        // Map Entity to DTO using AutoMapper
        var userDto = _mapper.Map<UserDto>(createdUser);
        userDto.UserTypeName = "Admin";

        // Cargar roles asignados para incluir en la respuesta
        if (createUserDto.RoleIds != null && createUserDto.RoleIds.Any())
        {
            userDto.Roles = await LoadUserRoles(createdUser.Id, cancellationToken);
        }

        return Result.Success(userDto);
    }

    /// <summary>
    /// Asigna m�ltiples roles a un usuario reci�n creado
    /// </summary>
    private async Task AssignRolesToUser(Guid userId, List<Guid> roleIds, CancellationToken cancellationToken)
    {
        // Validar que todos los roles existen
        var validRoles = new List<Role>();
        foreach (var roleId in roleIds)
        {
            var role = await _roleRepository.Find(r => r.Id == roleId && r.Status, cancellationToken);
            if (role != null)
            {
                validRoles.Add(role);
            }
        }

        // Asignar los roles v�lidos al usuario
        if (validRoles.Any())
        {
            foreach (var role in validRoles)
            {
                await _userRoleRepository.AssignRoleToUserAsync(userId, role.Id, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Carga los roles del usuario para incluir en la respuesta
    /// </summary>
    private async Task<List<RoleDropdownDto>> LoadUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.GetUserRolesWithDetailsAsync(userId, cancellationToken);
        
        return userRoles
            .Where(ur => ur.Role != null && ur.Role.Status)
            .Select(ur => new RoleDropdownDto
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name
            })
            .ToList();
    }
}
