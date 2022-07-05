using Data;
using Datas.Entities;

namespace Repositories;

public interface IGroupRepository : IRepositoryBase<Group>
{
    Group GetByUser(long userId);
}

public class GroupRepository : RepositoryBase<Group>, IGroupRepository
{
    private readonly DataContext _dbContext;

    public GroupRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Group GetByUser(long userId)
    {
        var groupUser = _dbContext.GroupUsers.FirstOrDefault(x => x.UserId == userId);

        if (groupUser == null)
            return null;

        return _dbContext.Groups.FirstOrDefault(x => x.GroupId == groupUser.GroupId);
    }
}
