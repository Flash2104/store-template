using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Contracts;
using AirSoft.Service.Repositories;

namespace AirSoft.Service.Implementations;

public class DataService : IDataService
{
    private readonly IDbContext _dbContext;

    private UserRepository? _users;
    private MemberRepository? _members;
    private TeamRepository? _teams;
    private GenericRepository<DbUserRole>? _userRoles;
    private GenericRepository<DbUserNavigation>? _userNavigations;
    private GenericRepository<DbNavigationItem>? _navigationItems;
    private CitiesRepository? _citiesRepository;

    public DataService(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public UserRepository Users => _users ??= new UserRepository(_dbContext);

    public MemberRepository Member => _members ??= new MemberRepository(_dbContext);

    public TeamRepository Team => _teams ??= new TeamRepository(_dbContext);

    public GenericRepository<DbUserRole> UserRoles => _userRoles ??= new GenericRepository<DbUserRole>(_dbContext);

    public GenericRepository<DbUserNavigation> UserNavigations => _userNavigations ??= new GenericRepository<DbUserNavigation>(_dbContext);
    
    public GenericRepository<DbNavigationItem> NavigationItems => _navigationItems ??= new GenericRepository<DbNavigationItem>(_dbContext);

    public CitiesRepository Cities => _citiesRepository ??= new CitiesRepository(_dbContext);

    public async Task<bool> SaveAsync()
    {
        await _dbContext.SaveAsync().ConfigureAwait(false);
        return true;
    }
}