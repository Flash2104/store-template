using Store.Data;
using Store.Data.Entity.Product;

namespace Store.Service.Repositories;

public class ProductRepository : GenericRepository<DbProduct>
{
    public ProductRepository(IDbContext context) : base(context)
    {
    }
}