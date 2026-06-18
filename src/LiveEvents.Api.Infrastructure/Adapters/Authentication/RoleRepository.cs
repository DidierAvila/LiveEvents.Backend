using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Authentication;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public RoleRepository(LiveEventsDbContext context, ILogger<RoleRepository> logger) : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<Role> QueryWithDetails()
    {
        return _dbContext.Roles
            .AsNoTracking()
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .Include(r => r.Users)
            .AsQueryable();
    }
}
