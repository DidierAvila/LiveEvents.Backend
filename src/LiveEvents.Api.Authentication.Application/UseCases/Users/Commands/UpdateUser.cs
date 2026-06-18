using AutoMapper;
using LiveEvents.Api.Authentication.Application.Security;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;
using LiveEvents.Api.Domain.Ports.Authentication;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;

public class UpdateUser
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly ISecurityStampService _securityStampService;
    private readonly IMapper _mapper;
    private readonly IValidationService _validationService;

    public UpdateUser(
        IRepositoryBase<User> userRepository,
        IUserRoleRepository userRoleRepository,
        ISecurityStampService securityStampService,
        IMapper mapper,
        IValidationService validationService)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _securityStampService = securityStampService;
        _mapper = mapper;
        _validationService = validationService;
    }

    public async Task<Result<UserDto>> HandleAsync(Guid id, UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validationService.ValidateAsync(
            updateUserDto,
            "User.InvalidData",
            "Los datos del usuario no son válidos.",
            cancellationToken);
        if (validationResult.IsFailure)
            return Result.Failure<UserDto>(validationResult.Error);

        // Find existing user
        var user = await _userRepository.Find(x => x.Id == id, cancellationToken);
        if (user == null)
            return Result.Failure<UserDto>(Error.NotFound("User.NotFound", "User not found"));

        // Map DTO properties to existing entity using AutoMapper
        _mapper.Map(updateUserDto, user);

        // Ensure UpdatedAt is set
        user.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        // Update in repository
        await _userRepository.Update(user, cancellationToken);

        // Gestionar roles si se proporcionaron
        if (updateUserDto.RoleIds != null)
        {
            await UpdateUserRoles(user.Id, updateUserDto.RoleIds, cancellationToken);
        }

        await _securityStampService.RefreshSecurityStampAsync(user.Id, cancellationToken);

        // Map Entity to DTO using AutoMapper
        var userDto = _mapper.Map<UserDto>(user);
        userDto.UserTypeName = "Admin";

        // Cargar roles del usuario
        userDto.Roles = await LoadUserRoles(user.Id, cancellationToken);

        return Result.Success(userDto);
    }

    private async Task<List<RoleDropdownDto>> LoadUserRoles(Guid userId, CancellationToken cancellationToken)
    {
        var userRoles = await _userRoleRepository.GetUserRolesWithDetailsAsync(userId, cancellationToken);
        return _mapper.Map<List<RoleDropdownDto>>(userRoles);
    }

    private async Task UpdateUserRoles(Guid userId, List<Guid> newRoleIds, CancellationToken cancellationToken)
    {
        // Remover todos los roles actuales del usuario
        await _userRoleRepository.RemoveAllUserRolesAsync(userId, cancellationToken);

        // Asignar los nuevos roles
        if (newRoleIds != null && newRoleIds.Any())
        {
            foreach (var roleId in newRoleIds)
            {
                await _userRoleRepository.AssignRoleToUserAsync(userId, roleId, cancellationToken);
            }
        }
    }
}
