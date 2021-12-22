using Store.Data.Entity;
using Store.Service.Repositories;

namespace Store.Service.Contracts;

public interface IDataService
{
    UserRepository Users { get; }

    GenericRepository<DbUserRole> UserRoles { get; }

    GenericRepository<DbUserNavigation> UserNavigations { get; }

    GenericRepository<DbNavigationItem> NavigationItems { get; }

    public CitiesRepository Cities { get; }

    Task<bool> SaveAsync();
}