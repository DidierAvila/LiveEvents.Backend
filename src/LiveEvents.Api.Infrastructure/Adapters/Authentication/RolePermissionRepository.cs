using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports.Authentication;
using LiveEvents.Api.Infrastructure.DbContexts;

namespace LiveEvents.Api.Infrastructure.Adapters.Authentication;

public class RolePermissionRepository : RepositoryBase<RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(LiveEventsDbContext context, ILogger<RepositoryBase<RolePermission>> logger) 
        : base(context, logger)
    {
    }

    public async Task<IEnumerable<RolePermission>> GetPermissionsByRoleIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .Where(x => x.RoleId == roleId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<RolePermission>> GetRolesByPermissionIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .Where(x => x.PermissionId == permissionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<RolePermission?> GetByRoleAndPermissionAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .AnyAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, cancellationToken);
    }

    public async Task<IEnumerable<RolePermission>> GetRolePermissionsWithDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .Include(x => x.Role)
            .Include(x => x.Permission)
            .ToListAsync(cancellationToken);
    }

    public async Task RemovePermissionFromRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var rolePermission = await EntitySet
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, cancellationToken);

        if (rolePermission != null)
        {
            EntitySet.Remove(rolePermission);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task RemoveAllPermissionsFromRoleAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        var rolePermissions = await EntitySet
            .Where(x => x.RoleId == roleId)
            .ToListAsync(cancellationToken);

        EntitySet.RemoveRange(rolePermissions);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RolePermission?> GetByCompositeIdAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await EntitySet
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId, cancellationToken);
    }
}
