using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Accounts.Handlers;
using LiveEvents.Api.Authentication.Application.UseCases.Mappings;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Handlers;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Mappings;
using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Mappings;
using LiveEvents.Api.Authentication.Application.UseCases.RolePermissions.Queries;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Handlers;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Mappings;
using LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Commands;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Handlers;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Mappings;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Queries;
using LiveEvents.Api.Authentication.Application.UseCases.UserTypes.Mappings;
using LiveEvents.Api.Authentication.Application.Security;
using LiveEvents.Api.Authentication.Application.Validation;
using LiveEvents.Api.Authentication.Features.Commands.RolePermissions;
using LiveEvents.Api.Common.Controllers;
using LiveEvents.Api.Common.Errors;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Infrastructure;

namespace LiveEvents.Api.Authentication.Application;

public static class AuthenticationApplicationDependencyInjection
{
    public static IServiceCollection AddAuthenticationApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // AutoMapper
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<RoleProfile>();
            cfg.AddProfile<AccountProfile>();
            cfg.AddProfile<PermissionProfile>();
            cfg.AddProfile<RolePermissionProfile>();
            cfg.AddProfile<UserTypeProfile>();
        });

        services.AddValidatorsFromAssemblyContaining<ValidationService>();
        services.AddScoped<IValidationService, ValidationService>();

        services.AddScoped<ISecurityStampService, SecurityStampService>();

        // Accounts
        services.AddScoped<LoginCommand>();
        services.AddScoped<ChangePassword>();
        services.AddScoped<TokenCommand>();
        services.AddScoped<ITokenCommandHandler, TokenCommandHandler>();
        services.AddScoped<IAccountsCommandHandler, AccountsCommandHandler>();

        // Users Command
        services.AddScoped<CreateUser>();
        services.AddScoped<UpdateUser>();
        services.AddScoped<DeleteUser>();
        services.AddScoped<UpdateUserAdditionalData>();
        services.AddScoped<IUserCommandHandler, UserCommandHandler>();

        // Users Query
        services.AddScoped<GetUserById>();
        services.AddScoped<GetAllUsers>();
        services.AddScoped<GetAllUsersBasic>();
        services.AddScoped<GetUserMe>();
        services.AddScoped<GetAllUsersFiltered>();
        services.AddScoped<IPaginationServiceBase<User, UserBasicDto, UserFilterDto>, UserPaginationService>();
        services.AddScoped<IUserQueryHandler, UserQueryHandler>();

        services.AddScoped<ICrudService<PaginationResponseDto<UserBasicDto>, UserDto, CreateUserDto, UpdateUserDto, UserFilterDto>,
            UsersCrudService>();

        // Roles Command
        services.AddScoped<CreateRole>();
        services.AddScoped<UpdateRole>();
        services.AddScoped<DeleteRole>();
        services.AddScoped<RemoveMultiplePermissionsFromRole>();
        services.AddScoped<IRoleCommandHandler, RoleCommandHandler>();

        // Roles Query
        services.AddScoped<GetRoleById>();
        services.AddScoped<GetAllRoles>();
        services.AddScoped<GetRolesDropdown>();
        services.AddScoped<GetAllRolesFiltered>();
        services.AddScoped<IPaginationServiceBase<Role, RoleListResponseDto, RoleFilterDto>, RolePaginationService>();
        services.AddScoped<IRoleQueryHandler, RoleQueryHandler>();

        // RolePermission Commands
        services.AddScoped<AssignPermissionToRole>();
        services.AddScoped<AssignMultiplePermissionsToRole>();
        services.AddScoped<RemovePermissionFromRole>();

        // RolePermission Queries
        services.AddScoped<GetAllRolePermissions>();
        services.AddScoped<GetRolesByPermission>();
        services.AddScoped<GetPermissionsByRole>();

        // Permissions Command
        services.AddScoped<CreatePermission>();
        services.AddScoped<UpdatePermission>();
        services.AddScoped<DeletePermission>();
        services.AddScoped<IPermissionCommandHandler, PermissionCommandHandler>();

        // Permissions Query
        services.AddScoped<GetPermissionById>();
        services.AddScoped<GetAllPermissions>();
        services.AddScoped<GetActivePermissions>();
        services.AddScoped<GetPermissionsSummary>();
        services.AddScoped<GetAllPermissionsFiltered>();
        services.AddScoped<GetPermissionsForDropdown>();
        services.AddScoped<IPaginationServiceBase<Permission, PermissionListResponseDto, PermissionFilterDto>, PermissionPaginationService>();
        services.AddScoped<IPermissionQueryHandler, PermissionQueryHandler>();

        // RolePermissions Query
        services.AddScoped<GetPermissionsByRole>();

        // Configure Infrastructure dependency injection
        services.AddInfrastructure(configuration);

        return services;
    }

    private sealed class UsersCrudService : ICrudService<PaginationResponseDto<UserBasicDto>, UserDto, CreateUserDto, UpdateUserDto, UserFilterDto>
    {
        private readonly IUserQueryHandler _userQueryHandler;
        private readonly IUserCommandHandler _userCommandHandler;

        public UsersCrudService(IUserQueryHandler userQueryHandler, IUserCommandHandler userCommandHandler)
            => (_userQueryHandler, _userCommandHandler) = (userQueryHandler, userCommandHandler);

        public Task<Result<PaginationResponseDto<UserBasicDto>>> GetAllAsync(UserFilterDto filter, CancellationToken cancellationToken)
            => _userQueryHandler.GetAllUsersFiltered(filter, cancellationToken);

        public Task<Result<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
            => _userQueryHandler.GetUserById(id, cancellationToken);

        public Task<Result<UserDto>> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken)
            => _userCommandHandler.CreateUser(createDto, cancellationToken);

        public Task<Result<UserDto>> UpdateAsync(Guid id, UpdateUserDto updateDto, CancellationToken cancellationToken)
            => _userCommandHandler.UpdateUser(id, updateDto, cancellationToken);

        public Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
            => _userCommandHandler.DeleteUser(id, cancellationToken);
    }
}
