using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entity.Category;

namespace Store.Data.Entity.Product;

public class DbProductsToCategories
{
    public DbProductsToCategories()
    {
    }

    public int? CategoryId { get; set; }

    public virtual DbCategory? Category { get; set; }

    public int? ProductId { get; set; }

    public virtual DbProduct? Product { get; set; }
}

internal sealed class DbProductsToCategoriesMapping
{
    public void Map(EntityTypeBuilder<DbProductsToCategories> builder)
    {
        builder.ToTable("ProductsToCategories");

        builder.HasKey(x => new { x.CategoryId, x.ProductId });

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductsToCategories)
            .HasForeignKey(x => x.ProductId);


        builder
            .HasOne(x => x.Category)
            .WithMany(x => x.ProductsToCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}