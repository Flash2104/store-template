using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity;

public class DbUserRole : DbEntity<int>
{
    public string Role { get; set; } = null!;

    public virtual List<DbUser>? Users { get; set; }

    public virtual List<DbUsersToRoles>? UsersToRoles { get; set; }

}

public enum UserRoleType
{
    None = 0,
    Administrator = 1,
    Manager = 2,
    Customer = 4
}

internal sealed class DbUserRolesMapping
{
    public void Map(EntityTypeBuilder<DbUserRole> builder)
    {
        builder.ToTable("UserRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Role).IsRequired().HasMaxLength(255);
        builder.HasMany(x => x.Users).WithMany(x => x.UserRoles).UsingEntity<DbUsersToRoles>();

        var roles = Enum.GetValues<UserRoleType>().Where(x => x != UserRoleType.None).Select(v => new DbUserRole { Id = (int)v, Role = v.ToString() })
            .ToArray();
        builder.HasData(roles);
    }
}