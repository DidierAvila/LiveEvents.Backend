using LiveEvents.Api.Authentication.Application.UseCases.Roles.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Infrastructure.Adapters.Pagination;

namespace LiveEvents.Api.Authentication.Application.UseCases.Roles.Queries;

public sealed class RolePaginationService : PaginationServiceBase<Role, RoleListResponseDto, RoleFilterDto>
{
    protected override IQueryable<Role> ApplyFilters(IQueryable<Role> query, RoleFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.ToLower();
            query = query.Where(r =>
                r.Name.ToLower().Contains(searchTerm) ||
                (r.Description != null && r.Description.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.ToLower();
            query = query.Where(r => r.Name.ToLower().Contains(name));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(r => r.Status == filter.Status.Value);
        }

        return query;
    }

    protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, string? sortBy, bool sortDescending)
    {
        return SortingHelper.CreateSortingBuilder(query)
            .AddSortMapping("name", r => r.Name)
            .AddSortMapping("description", r => r.Description ?? string.Empty)
            .AddSortMapping("status", r => r.Status)
            .AddSortMapping("createdat", r => r.CreatedAt)
            .SetDefaultSort(r => r.Name)
            .ApplySorting(sortBy, sortDescending);
    }

    protected override Task<IEnumerable<RoleListResponseDto>> MapToDto(IEnumerable<Role> entities, CancellationToken cancellationToken)
    {
        var roleDtos = entities.Select(r => new RoleListResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            Status = r.Status,
            UserCount = r.Users?.Count ?? 0,
            PermissionCount = r.RolePermissions?.Count ?? 0,
            CreatedAt = r.CreatedAt
        }).ToList();

        return Task.FromResult<IEnumerable<RoleListResponseDto>>(roleDtos);
    }
}
