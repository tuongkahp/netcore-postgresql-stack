using Data;
using Datas.Entities;

namespace Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    List<Role> GetRoles(long userId);
    User GetById(long userId);
    Task UpdateRoles(long userId, List<int> roleIds);
    Task UpdateGroups(long userId, List<long> groupIds);
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly DataContext _dbContext;

    public UserRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public User GetById(long userId)
    {
        return _dbContext.Users.FirstOrDefault(x => x.UserId == userId);
    }

    public List<Role> GetRoles(long userId)
    {
        // Get roles from group user
        var qGroupRoles = (from g in _dbContext.Groups
                           join g_u in _dbContext.GroupUsers.Where(x => x.UserId == userId) on g.GroupId equals g_u.GroupId
                           join g_r in _dbContext.GroupRoles on g.GroupId equals g_r.GroupId
                           join r in _dbContext.Roles on g_r.RoleId equals r.RoleId
                           select r);

        // Get roles from group user
        var qUserRoles = (from u_r in _dbContext.UserRoles.Where(x => x.UserId == userId)
                          join r in _dbContext.Roles on u_r.RoleId equals r.RoleId
                          select r);

        return qGroupRoles.Union(qUserRoles).ToList();
    }

    public async Task UpdateRoles(long userId, List<int> roleIds)
    {
        var userRoles = _dbContext.UserRoles.Where(x => x.UserId == userId).ToList();
        if (userRoles.Count > 0)
            _dbContext.UserRoles.RemoveRange(userRoles);

        await _dbContext.UserRoles.AddRangeAsync(roleIds.Select(x => new UserRole()
        {
            RoleId = x,
            UserId = userId,
        }));
    }

    public async Task UpdateGroups(long userId, List<long> groupIds)
    {
        var groupUser = _dbContext.GroupUsers.Where(x => x.UserId == userId).ToList();

        if (groupUser.Count > 0)
            _dbContext.GroupUsers.RemoveRange(groupUser);

        await _dbContext.GroupUsers.AddRangeAsync(groupIds.Select(x => new GroupUser()
        {
            GroupId = x,
            UserId = userId,
        }));
    }
}