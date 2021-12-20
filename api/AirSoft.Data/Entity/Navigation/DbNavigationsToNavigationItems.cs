
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbNavigationsToNavigationItems
{
    public Guid NavigationId { get; set; }
    public virtual DbUserNavigation? Navigation { get; set; }

    public int NavigationItemId { get; set; }
    public virtual DbNavigationItem? NavigationItem { get; set; }
}

internal sealed class DbNavigationsToNavigationItemsMapping
{
    public void Map(EntityTypeBuilder<DbNavigationsToNavigationItems> builder)
    {
        builder.ToTable("NavigationsToNavigationItems");

        builder.HasKey(x => new { x.NavigationId, x.NavigationItemId });
        builder
             .HasOne(x => x.Navigation)
             .WithMany(x => x.NavigationsToNavigationItems)
             .HasForeignKey(j => j.NavigationId);
        builder
            .HasOne(x => x.NavigationItem)
            .WithMany(x => x.NavigationsToNavigationItems)
            .HasForeignKey(j => j.NavigationItemId);
    }
}
