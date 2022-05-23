
using Data;
using Repositories;

namespace Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task<int> SaveChangeAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;
    private IUserRepository _user;
   
    public UnitOfWork(DataContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public IUserRepository Users => _user ?? (_user = new UserRepository(_dbContext));

    public async Task<int> SaveChangeAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}