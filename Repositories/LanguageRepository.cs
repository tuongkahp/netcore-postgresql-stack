using Data;
using Datas.Entities;

namespace Repositories;

public interface ILanguageRepository : IRepositoryBase<Language>
{
}

public class LanguageRepository : RepositoryBase<Language>, ILanguageRepository
{
    private readonly DataContext _dbContext;

    public LanguageRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}