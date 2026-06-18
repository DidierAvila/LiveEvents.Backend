using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Features.Pagination.Dtos;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Handlers;

public class RoleQueryHandler : IRoleQueryHandler
{
    private readonly GetRoleById _getRoleById;
    private readonly GetAllRoles _getAllRoles;
    private readonly GetAllRolesFiltered _getAllRolesFiltered;
    private readonly GetRolesDropdown _getRolesDropdown;
    private readonly IValidationService _validationService;

    public RoleQueryHandler(
        GetRoleById getRoleById,
        GetAllRoles getAllRoles,
        GetAllRolesFiltered getAllRolesFiltered,
        GetRolesDropdown getRolesDropdown,
        IValidationService validationService)
    {
        _getRoleById = getRoleById;
        _getAllRoles = getAllRoles;
        _getAllRolesFiltered = getAllRolesFiltered;
        _getRolesDropdown = getRolesDropdown;
        _validationService = validationService;
    }

    public async Task<Result<RoleDto>> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var role = await _getRoleById.HandleAsync(id, cancellationToken);
            if (role == null)
            {
                return Result.Failure<RoleDto>(Error.NotFound("Role.NotFound", $"Rol con ID {id} no encontrado"));
            }
            return Result.Success(role);
        }
        catch (Exception ex)
        {
            return Result.Failure<RoleDto>(Error.Failure("Role.GetById", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<RoleDto>>> GetAllRoles(CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _getAllRoles.HandleAsync(cancellationToken);
            return Result.Success(roles);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<RoleDto>>(Error.Failure("Role.GetAll", ex.Message));
        }
    }

    public async Task<Result<PaginationResponseDto<RoleListResponseDto>>> GetAllRolesFiltered(RoleFilterDto filter, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(
                filter,
                "Role.InvalidFilter",
                "Los filtros de roles no son válidos.",
                cancellationToken);
            if (validationResult.IsFailure)
                return Result.Failure<PaginationResponseDto<RoleListResponseDto>>(validationResult.Error);

            var roles = await _getAllRolesFiltered.GetRolesFiltered(filter, cancellationToken);
            return Result.Success(roles);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PaginationResponseDto<RoleListResponseDto>>(Error.Validation("Role.InvalidFilter", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginationResponseDto<RoleListResponseDto>>(Error.Failure("Role.GetFiltered", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<RoleDropdownDto>>> GetRolesDropdown(CancellationToken cancellationToken)
    {
        try
        {
            var roles = await _getRolesDropdown.HandleAsync(cancellationToken);
            return Result.Success(roles);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<RoleDropdownDto>>(Error.Failure("Role.GetDropdown", ex.Message));
        }
    }
}
