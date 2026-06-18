using Microsoft.EntityFrameworkCore;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Common.Features.Pagination.Dtos;
using System.Linq.Expressions;

namespace LiveEvents.Api.Infrastructure.Adapters.Pagination;

public abstract class PaginationServiceBase<TEntity, TDto, TFilter> : IPaginationServiceBase<TEntity, TDto, TFilter>
    where TEntity : class
    where TFilter : PaginationRequestDto
{
    public async Task<PaginationResponseDto<TDto>> GetPaginatedAsync(
        IQueryable<TEntity> queryable,
        TFilter filter,
        CancellationToken cancellationToken)
    {
        ValidateAndNormalizePaginationParams(filter);

        var filteredQuery = ApplyFilters(queryable, filter);
        var totalRecords = await filteredQuery.CountAsync(cancellationToken);
        var sortedQuery = ApplySorting(filteredQuery, filter.SortBy, filter.SortDescending);
        var paginatedEntities = await sortedQuery
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = await MapToDto(paginatedEntities, cancellationToken);
        return dtos.ToPaginatedResult(filter.Page, filter.PageSize, totalRecords, filter.SortBy);
    }

    public async Task<PaginationResponseDto<TDto>> GetPaginatedWithExpressionAsync(
        IQueryable<TEntity> queryable,
        TFilter filter,
        Expression<Func<TEntity, bool>>? additionalFilter = null,
        CancellationToken cancellationToken = default)
    {
        if (additionalFilter != null)
        {
            queryable = queryable.Where(additionalFilter);
        }

        return await GetPaginatedAsync(queryable, filter, cancellationToken);
    }

    protected abstract IQueryable<TEntity> ApplyFilters(IQueryable<TEntity> query, TFilter filter);
    protected abstract IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string? sortBy, bool sortDescending);
    protected abstract Task<IEnumerable<TDto>> MapToDto(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    private static void ValidateAndNormalizePaginationParams(TFilter filter)
    {
        if (filter.Page <= 0) filter.Page = 1;
        if (filter.PageSize <= 0) filter.PageSize = 10;
        if (filter.PageSize > 100) filter.PageSize = 100;
    }
}
