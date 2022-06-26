using Data;
using Datas.Entities;

namespace Repositories;

public interface IUserTokenRepository : IRepositoryBase<UserToken>
{
}

public class UserTokenRepository : RepositoryBase<UserToken>, IUserTokenRepository
{
    private readonly DataContext _dbContext;

    public UserTokenRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}