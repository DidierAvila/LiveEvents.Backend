using LiveEvents.Api.Domain.Entities.Authentication;

namespace LiveEvents.Api.Domain.Ports.Authentication;

public interface IUserRepository : IRepositoryBase<User>
{
    IQueryable<User> QueryWithDetails();
}
