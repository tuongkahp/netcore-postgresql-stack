using Data;
using Datas.Entities;

namespace Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly DataContext _dbContext;

    public UserRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
