using Data;
using Datas.Entities;

namespace Repositories;

public interface IRoleRepository : IRepositoryBase<Role>
{
}

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly DataContext _dbContext;

    public RoleRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
