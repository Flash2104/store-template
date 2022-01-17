using Store.Data;
using Store.Data.Entity.Category;

namespace Store.Service.Repositories;

public class CategoryTreeRepository : GenericRepository<DbCategoryTree>
{
    public CategoryTreeRepository(IDbContext context) : base(context)
    {
    }
}