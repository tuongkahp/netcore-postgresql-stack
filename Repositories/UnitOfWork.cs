
using Data;
using Repositories;

namespace Repositories;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IRoleRepository Roles { get; }
    IGroupRepository Groups { get; }
    ITranslationRepository Translations { get; }
    ILanguageRepository Languages { get; }
    IUserTokenRepository UserTokens { get; }
    Task<int> SaveChangeAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dbContext;
    private IUserRepository _user;
    private IRoleRepository _role;
    private IGroupRepository _group;
    private ITranslationRepository _translation;
    private ILanguageRepository _language;
    private IUserTokenRepository _userToken;

    public UnitOfWork(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IUserRepository Users => _user ?? (_user = new UserRepository(_dbContext));
    public IRoleRepository Roles => _role ?? (_role = new RoleRepository(_dbContext));
    public IGroupRepository Groups => _group ?? (_group = new GroupRepository(_dbContext));
    public ITranslationRepository Translations => _translation ?? (_translation = new TranslationRepository(_dbContext));
    public ILanguageRepository Languages => _language ?? (_language = new LanguageRepository(_dbContext));
    public IUserTokenRepository UserTokens => _userToken ?? (_userToken = new UserTokenRepository(_dbContext));

    public async Task<int> SaveChangeAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}