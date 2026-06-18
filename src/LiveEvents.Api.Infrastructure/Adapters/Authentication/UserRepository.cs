using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Authentication;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public UserRepository(LiveEventsDbContext context, ILogger<UserRepository> logger) : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<User> QueryWithDetails()
    {
        return _dbContext.Users
            .AsNoTracking()
            .Include(u => u.UserType)
            .Include(u => u.Roles);
    }
}
