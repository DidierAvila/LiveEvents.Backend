using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Common.Errors;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Handlers;

public class PermissionQueryHandler : IPermissionQueryHandler
{
    private readonly GetPermissionById _getPermissionById;
    private readonly GetAllPermissions _getAllPermissions;
    private readonly GetActivePermissions _getActivePermissions;
    private readonly GetPermissionsSummary _getPermissionsSummary;
    private readonly GetAllPermissionsFiltered _getAllPermissionsFiltered;
    private readonly GetPermissionsForDropdown _getPermissionsForDropdown;
    private readonly IValidationService _validationService;

    public PermissionQueryHandler(
        GetPermissionById getPermissionById,
        GetAllPermissions getAllPermissions,
        GetActivePermissions getActivePermissions,
        GetPermissionsSummary getPermissionsSummary,
        GetAllPermissionsFiltered getAllPermissionsFiltered,
        GetPermissionsForDropdown getPermissionsForDropdown,
        IValidationService validationService)
    {
        _getPermissionById = getPermissionById;
        _getAllPermissions = getAllPermissions;
        _getActivePermissions = getActivePermissions;
        _getPermissionsSummary = getPermissionsSummary;
        _getAllPermissionsFiltered = getAllPermissionsFiltered;
        _getPermissionsForDropdown = getPermissionsForDropdown;
        _validationService = validationService;
    }

    public async Task<Result<PermissionDto>> GetPermissionById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var permission = await _getPermissionById.HandleAsync(id, cancellationToken);
            if (permission == null)
            {
                return Result.Failure<PermissionDto>(Error.NotFound("Permission.NotFound", $"Permiso con ID {id} no encontrado"));
            }
            return Result.Success(permission);
        }
        catch (Exception ex)
        {
            return Result.Failure<PermissionDto>(Error.Failure("Permission.GetById", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<PermissionDto>>> GetAllPermissions(CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _getAllPermissions.HandleAsync(cancellationToken);
            return Result.Success(permissions);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PermissionDto>>(Error.Failure("Permission.GetAll", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<PermissionDto>>> GetActivePermissions(CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _getActivePermissions.HandleAsync(cancellationToken);
            return Result.Success(permissions);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PermissionDto>>(Error.Failure("Permission.GetActive", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<PermissionSummaryDto>>> GetPermissionsSummary(CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _getPermissionsSummary.HandleAsync(cancellationToken);
            return Result.Success(permissions);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PermissionSummaryDto>>(Error.Failure("Permission.GetSummary", ex.Message));
        }
    }

    public async Task<Result<PaginationResponseDto<PermissionListResponseDto>>> GetAllPermissionsFiltered(PermissionFilterDto filter, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validationService.ValidateAsync(
                filter,
                "Permission.InvalidFilter",
                "Los filtros de permisos no son válidos.",
                cancellationToken);
            if (validationResult.IsFailure)
                return Result.Failure<PaginationResponseDto<PermissionListResponseDto>>(validationResult.Error);

            var permissions = await _getAllPermissionsFiltered.GetPermissionsFiltered(filter, cancellationToken);
            return Result.Success(permissions);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure<PaginationResponseDto<PermissionListResponseDto>>(Error.Validation("Permission.InvalidFilter", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginationResponseDto<PermissionListResponseDto>>(Error.Failure("Permission.GetFiltered", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<PermissionDropdownDto>>> GetPermissionsForDropdown(CancellationToken cancellationToken)
    {
        try
        {
            var permissions = await _getPermissionsForDropdown.HandleAsync(cancellationToken);
            return Result.Success(permissions);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<PermissionDropdownDto>>(Error.Failure("Permission.GetDropdown", ex.Message));
        }
    }
}
