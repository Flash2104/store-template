using Store.Data.Entity;
using Store.Service.Repositories;

namespace Store.Service.Contracts;

public interface IDataService
{
    UserRepository Users { get; }

    StoreRepository Store { get; }

    CategoryTreeRepository CategoryTrees { get; }

    CategoryTreeItemRepository CategoryTreeItems { get; }

    ProductRepository Products { get; }

    GenericRepository<DbUserRole> UserRoles { get; }

    public CitiesRepository Cities { get; }

    Task<bool> SaveAsync();
}