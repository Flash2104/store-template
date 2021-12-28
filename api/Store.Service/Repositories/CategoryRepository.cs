using Store.Data;
using Store.Data.Entity.Category;

namespace Store.Service.Repositories;

public class CategoryRepository : GenericRepository<DbCategory>
{
    public CategoryRepository(IDbContext context) : base(context)
    {
    }
}