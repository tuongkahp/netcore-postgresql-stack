using Data;
using Datas.Entities;

namespace Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    List<Role> GetRoles(long userId);
    User GetById(long userId);
    List<User> GetAll();
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

    public List<User> GetAll()
    {
        var lstUsers = new List<User>();

        for (int i = 1; i < 521; i++)
        {
            lstUsers.Add(new User()
            {
                UserId = i,
                Username = "user" + i,
                Email = "user" + i + "@gmail.com",
                FullName = "Name" + i,
                Status = Constants.Enums.UserStatus.Active
            });
        }

        return lstUsers;
    }
}