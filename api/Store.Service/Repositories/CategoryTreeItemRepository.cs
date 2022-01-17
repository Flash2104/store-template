using Store.Data;
using Store.Data.Entity.Category;

namespace Store.Service.Repositories;

public class CategoryTreeItemRepository : GenericRepository<DbCategoryItem>
{
    public CategoryTreeItemRepository(IDbContext context) : base(context)
    {
    }
}