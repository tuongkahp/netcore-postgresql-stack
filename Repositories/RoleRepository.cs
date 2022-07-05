using Data;
using Datas.Entities;
using Dtos.Users;

namespace Repositories;

public interface IRoleRepository : IRepositoryBase<Role>
{
    List<RoleDto> GetByUser(long userId);
    List<RoleDto> GetByGroup(long groupId);
}

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    private readonly DataContext _dbContext;

    public RoleRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public List<RoleDto> GetByUser(long userId)
    {
        return (from ru in _dbContext.UserRoles.Where(x => x.UserId == userId)
                join r in _dbContext.Roles on ru.RoleId equals r.RoleId
                select new RoleDto()
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    IsGroupRole = false
                }).ToList();
    }

    public List<RoleDto> GetByGroup(long groupId)
    {
        return (from gr in _dbContext.GroupRoles.Where(x => x.GroupId == groupId)
                join r in _dbContext.Roles on gr.RoleId equals r.RoleId
                select new RoleDto()
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    IsGroupRole = true
                }).ToList();
    }
}
