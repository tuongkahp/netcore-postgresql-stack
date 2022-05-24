using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{
    int Add(TEntity Entity);
    Task<int> AddAsync(TEntity Entity);
    int AddRange(List<TEntity> Entities);
    IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate);
    int Update(TEntity Entity);
    int Delete(int Id);
    int DeleteRange(List<TEntity> items);
    int Save();
    //TEntity GetById(int Id);
    IQueryable<TEntity> GetAll();
}

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    private DataContext _dbContext;

    public RepositoryBase(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public virtual int Add(TEntity Entity)
    {
        _dbContext.Set<TEntity>().Add(Entity);
        var result = _dbContext.SaveChanges();
        return result;
    }

    public virtual async Task<int> AddAsync(TEntity Entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(Entity);
        var result = _dbContext.SaveChanges();
        return result;
    }

    public virtual int AddRange(List<TEntity> Entities)
    {
        _dbContext.Set<TEntity>().AddRange(Entities);
        var result = _dbContext.SaveChanges();
        return result;
    }

    public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>().Where(predicate);
    }

    public virtual int Delete(int Id)
    {
        TEntity Entity = _dbContext.Set<TEntity>().Find(Id);

        if (Entity != null)
        {
            _dbContext.Set<TEntity>().Remove(Entity);
            var result = _dbContext.SaveChanges();
            return result;
        }

        return -1;
    }

    //public virtual TEntity GetById(int Id)
    //{
    //    return _dbContext.Set<TEntity>().Find(Id);
    //}

    public virtual IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }

    public virtual int Update(TEntity Entity)
    {
        _dbContext.Set<TEntity>().Attach(Entity);
        _dbContext.Entry(Entity).State = EntityState.Modified;
        var result = _dbContext.SaveChanges();
        return result;
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }

    public int DeleteRange(List<TEntity> items)
    {
        _dbContext.Set<TEntity>().RemoveRange(items);
        return _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        if (_dbContext != null)
            _dbContext.Dispose();
    }
}
