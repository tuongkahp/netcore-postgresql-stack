using Data;
using Datas.Entities;

namespace Repositories;

public interface IGroupRepository : IRepositoryBase<Group>
{
}

public class GroupRepository : RepositoryBase<Group>, IGroupRepository
{
    private readonly DataContext _dbContext;

    public GroupRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
