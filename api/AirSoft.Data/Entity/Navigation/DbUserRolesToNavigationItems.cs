
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity;

public class DbUserRolesToNavigationItems
{
    public int RoleId { get; set; }
    public virtual DbUserRole? Role { get; set; }

    public int NavigationItemId { get; set; }
    public virtual DbNavigationItem? NavigationItem { get; set; }
}

internal sealed class DbUserRolesToNavigationItemsMapping
{
    public void Map(EntityTypeBuilder<DbUserRolesToNavigationItems> builder)
    {
        builder.ToTable("UserRolesToNavigationItems");

        builder.HasKey(x => new { x.RoleId, x.NavigationItemId });
        builder
             .HasOne(x => x.Role)
             .WithMany(x => x.UserRolesToNavigationItems)
             .HasForeignKey(j => j.RoleId);
        builder
            .HasOne(x => x.NavigationItem)
            .WithMany(x => x.UserRolesToNavigationItems)
            .HasForeignKey(j => j.NavigationItemId);
    }
}
