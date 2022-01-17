using Store.Data;
using Store.Data.Entity;
using Store.Service.Contracts;
using Store.Service.Repositories;

namespace Store.Service.Implementations;

public class DataService : IDataService
{
    private readonly IDbContext _dbContext;

    private UserRepository? _users;
    private StoreRepository? _store;
    private CategoryTreeRepository? _categoryTrees;
    private CategoryTreeItemRepository? _categoryItems;
    private ProductRepository? _products;
    private GenericRepository<DbUserRole>? _userRoles;
    private CitiesRepository? _citiesRepository;

    public DataService(IDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public UserRepository Users => _users ??= new UserRepository(_dbContext);

    public StoreRepository Store => _store ??= new StoreRepository(_dbContext);

    public CategoryTreeRepository CategoryTrees => _categoryTrees ??= new CategoryTreeRepository(_dbContext);
    public CategoryTreeItemRepository CategoryTreeItems => _categoryItems ??= new CategoryTreeItemRepository(_dbContext);

    public ProductRepository Products => _products ??= new ProductRepository(_dbContext);
    
    public GenericRepository<DbUserRole> UserRoles => _userRoles ??= new GenericRepository<DbUserRole>(_dbContext);
    
    public CitiesRepository Cities => _citiesRepository ??= new CitiesRepository(_dbContext);

    public async Task<bool> SaveAsync()
    {
        await _dbContext.SaveAsync().ConfigureAwait(false);
        return true;
    }
}