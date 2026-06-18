using LiveEvents.Api.Authentication.Application.UseCases.Permissions.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Infrastructure.Adapters.Pagination;

namespace LiveEvents.Api.Authentication.Application.UseCases.Permissions.Queries;

public sealed class PermissionPaginationService : PaginationServiceBase<Permission, PermissionListResponseDto, PermissionFilterDto>
{
    protected override IQueryable<Permission> ApplyFilters(IQueryable<Permission> query, PermissionFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var searchTerm = filter.Search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchTerm) ||
                (p.Description != null && p.Description.ToLower().Contains(searchTerm)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(p => p.Status == filter.Status.Value);
        }

        return query;
    }

    protected override IQueryable<Permission> ApplySorting(IQueryable<Permission> query, string? sortBy, bool sortDescending)
    {
        return SortingHelper.CreateSortingBuilder(query)
            .AddSortMapping("name", p => p.Name)
            .AddSortMapping("description", p => p.Description ?? string.Empty)
            .AddSortMapping("status", p => p.Status)
            .AddSortMapping("createdat", p => p.CreatedAt)
            .SetDefaultSort(p => p.Name)
            .ApplySorting(sortBy, sortDescending);
    }

    protected override Task<IEnumerable<PermissionListResponseDto>> MapToDto(IEnumerable<Permission> entities, CancellationToken cancellationToken)
    {
        var permissions = entities.Select(p => new PermissionListResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status,
            RoleCount = p.RolePermissions?.Count ?? 0,
            CreatedAt = p.CreatedAt
        }).ToList();

        return Task.FromResult<IEnumerable<PermissionListResponseDto>>(permissions);
    }
}
