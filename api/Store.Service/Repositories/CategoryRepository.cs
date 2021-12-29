using Store.Data;
using Store.Data.Entity.Category;

namespace Store.Service.Repositories;

public class CategoryRepository : GenericRepository<DbCategoryTree>
{
    public CategoryRepository(IDbContext context) : base(context)
    {
    }
}