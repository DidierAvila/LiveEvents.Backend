using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Common.Features.Pagination;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Infrastructure.Adapters.Pagination;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Queries;

public sealed class UserPaginationService : PaginationServiceBase<User, UserBasicDto, UserFilterDto>
{
    private readonly IMapper _mapper;

    public UserPaginationService(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected override IQueryable<User> ApplyFilters(IQueryable<User> query, UserFilterDto filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(u => u.Name.Contains(filter.Search) ||
                u.Email.Contains(filter.Search) ||
                (u.Phone != null && u.Phone.Contains(filter.Search)));
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(u => u.Name.Contains(filter.Name));
        }

        if (!string.IsNullOrWhiteSpace(filter.Email))
        {
            query = query.Where(u => u.Email.Contains(filter.Email));
        }

        if (filter.RoleId.HasValue)
        {
            query = query.Where(u => u.Roles.Any(r => r.Id == filter.RoleId.Value));
        }

        if (filter.UserTypeId.HasValue)
        {
            query = query.Where(u => u.UserTypeId == filter.UserTypeId.Value);
        }

        if (filter.CreatedAfter.HasValue)
        {
            query = query.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);
        }

        if (filter.CreatedBefore.HasValue)
        {
            query = query.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);
        }

        return query;
    }

    protected override IQueryable<User> ApplySorting(IQueryable<User> query, string? sortBy, bool sortDescending)
    {
        return SortingHelper.CreateSortingBuilder(query)
            .AddSortMapping("name", u => u.Name)
            .AddSortMapping("email", u => u.Email)
            .AddSortMapping("createdat", u => u.CreatedAt)
            .AddSortMapping("usertypeid", u => u.UserTypeId)
            .SetDefaultSort(u => u.Name)
            .ApplySorting(sortBy, sortDescending);
    }

    protected override Task<IEnumerable<UserBasicDto>> MapToDto(IEnumerable<User> entities, CancellationToken cancellationToken)
    {
        IEnumerable<UserBasicDto> users = _mapper.Map<IEnumerable<UserBasicDto>>(entities);
        return Task.FromResult(users);
    }
}
