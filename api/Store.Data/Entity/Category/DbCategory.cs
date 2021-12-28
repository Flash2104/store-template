using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entity.Product;

namespace Store.Data.Entity.Category;

public class DbCategory : DbEntity<int>
{
    public string Title { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public int? ParentId { get; set; }
    public int Order { get; set; }
    
    public virtual List<DbProductsToCategories>? ProductsToCategories { get; set; }
}

internal sealed class DbCategoryMapping
{
    public void Map(EntityTypeBuilder<DbCategory> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Icon).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);
    }
}