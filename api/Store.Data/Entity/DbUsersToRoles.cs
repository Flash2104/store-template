
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity;

public class DbUsersToRoles
{
    public Guid UserId { get; set; }
    public virtual DbUser? User { get; set; }

    public int RoleId { get; set; }
    public virtual DbUserRole? Role { get; set; }
}

internal sealed class DbUsersToRolesMapping
{
    public void Map(EntityTypeBuilder<DbUsersToRoles> builder)
    {
        builder.ToTable("UsersToRoles");

        builder.HasKey(x => new { x.UserId, x.RoleId });
        builder
             .HasOne(x => x.Role)
             .WithMany(x => x.UsersToRoles)
             .HasForeignKey(j => j.RoleId);
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UsersToRoles)
            .HasForeignKey(j => j.UserId);
    }
}
