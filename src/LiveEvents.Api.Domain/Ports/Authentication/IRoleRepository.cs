using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Domain.Ports.Authentication;

public interface IRoleRepository : IRepositoryBase<Role>
{
    IQueryable<Role> QueryWithDetails();
}
