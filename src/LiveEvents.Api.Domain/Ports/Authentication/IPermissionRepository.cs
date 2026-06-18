using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Domain.Ports.Authentication;

public interface IPermissionRepository : IRepositoryBase<Permission>
{
    IQueryable<Permission> QueryWithDetails();
}
