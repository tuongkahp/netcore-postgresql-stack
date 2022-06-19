using Data;
using Datas.Entities;

namespace Repositories;

public interface ITranslationRepository : IRepositoryBase<Translation>
{
}

public class TranslationRepository : RepositoryBase<Translation>, ITranslationRepository
{
    private readonly DataContext _dbContext;

    public TranslationRepository(DataContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}