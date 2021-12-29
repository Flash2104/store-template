using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity.Category;

public class DbCategoryTree : DbEntity<int>
{
    public string Title { get; set; } = null!;

    public bool IsDefault { get; set; }

    public int StoreId { get; set; }

    public virtual DbStore? Store { get; set; }

    public virtual List<DbCategoryItem>? CategoryItems { get; set; }
}

internal sealed class DbCategoryTreeMapping
{
    public void Map(EntityTypeBuilder<DbCategoryTree> builder)
    {
        builder.ToTable("CategoryTrees");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.IsDefault).IsRequired();
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);

        builder
            .HasOne(x => x.Store)
            .WithMany(x => x.CategoryTrees)
            .HasForeignKey(x => x.StoreId);

        builder.HasData(new DbCategoryTree()
        {
            Id = 1,
            CreatedBy = 1,
            ModifiedBy = 1,
            IsDefault = true,
            StoreId = 1,
            Title = "Категории по умолчанию",
            CreatedDate = new DateTime(2022, 12, 29, 13, 18, 00),
            ModifiedDate = new DateTime(2022, 12, 29, 13, 18, 00)
        });
    }
}