using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Entity.Category;

namespace Store.Data.Entity;

public class DbStore: DbEntity<int>
{
    public DbStore()
    {
    }

    private DbStore(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    public string Title { get; set; } = null!;

    public byte[] Logo { get; set; } = null!;

    public virtual List<DbUser>? Users { get; set; }

    public virtual List<DbCategoryTree>? CategoryTrees { get; set; }
}

internal sealed class DbStoreMapping
{
    public void Map(EntityTypeBuilder<DbStore> builder)
    {
        builder.ToTable("Store");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Logo).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var store = new DbStore()
        {
            Id = 1,
            CreatedDate = new DateTime(2022, 12, 28, 14, 00, 00),
            ModifiedDate = new DateTime(2022, 12, 28, 14, 00, 00),
            CreatedBy = 1,
            ModifiedBy = 1,
            Title = "Test Store",
            Logo = File.ReadAllBytes(root + "\\InitialData\\store-logo.png")
        };
        builder.HasData(store);
    }
}
