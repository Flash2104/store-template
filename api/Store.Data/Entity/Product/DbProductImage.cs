using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity.Product;
public class DbProductImage : DbEntity<int>
{
    public DbProductImage()
    {
    }
    
    public string Title { get; set; } = null!;

    public byte[] Buffer { get; set; } = null!;
    
    public int? ProductId { get; set; }

    public virtual DbProduct? Product { get; set; }
}

internal sealed class DbProductImageMapping
{
    public void Map(EntityTypeBuilder<DbProductImage> builder)
    {
        builder.ToTable("ProductImages");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Buffer).IsRequired().HasMaxLength(5097152);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);

        builder
            .HasOne(x => x.Product)
            .WithMany(x => x.ProductImages)
            .HasForeignKey(x => x.ProductId);
    }
}