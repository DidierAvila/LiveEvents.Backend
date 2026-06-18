using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Authentication;

public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
{
    private readonly LiveEventsDbContext _dbContext;

    public PermissionRepository(LiveEventsDbContext context, ILogger<PermissionRepository> logger) : base(context, logger)
    {
        _dbContext = context;
    }

    public IQueryable<Permission> QueryWithDetails()
    {
        return _dbContext.Permissions
            .AsNoTracking()
            .Include(p => p.RolePermissions)
            .ThenInclude(rp => rp.Role);
    }
}
