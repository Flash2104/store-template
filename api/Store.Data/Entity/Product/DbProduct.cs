using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity.Product;

public class DbProduct : DbEntity<int>
{
    public DbProduct()
    {
    }

    private DbProduct(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;
    
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public decimal PriceAmount { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime? SupplyDate { get; set; }

    public int? SupplyQuantity { get; set; }

    public virtual List<DbProductImage>? ProductImages { get; set; }

    public virtual List<DbProductsToCategories>? ProductsToCategories { get; set; }
}

internal sealed class DbProductMapping
{
    public void Map(EntityTypeBuilder<DbProduct> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.PriceAmount).IsRequired().HasPrecision(19, 4);
        // builder.Property(x => x.PriceCurrencyIso).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
    }
}