using LiveEvents.Api.Common.Features.Pagination.Dtos;
using System.Linq.Expressions;

namespace LiveEvents.Api.Common.Features.Pagination;

public interface IPaginationServiceBase<TEntity, TDto, TFilter>
    where TEntity : class
    where TFilter : PaginationRequestDto
{
    Task<PaginationResponseDto<TDto>> GetPaginatedWithExpressionAsync(IQueryable<TEntity> queryable, TFilter filter, Expression<Func<TEntity, bool>>? additionalFilter = null, CancellationToken cancellationToken = default);
    Task<PaginationResponseDto<TDto>> GetPaginatedAsync(IQueryable<TEntity> queryable, TFilter filter, CancellationToken cancellationToken);
}
