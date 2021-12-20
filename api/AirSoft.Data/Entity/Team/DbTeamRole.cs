using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeamRole : DbEntity<Guid>
{
    public DbTeamRole()
    {
    }
    private DbTeamRole(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; } = null!;

    public string Title { get; set; } = null!;

    public int Rank { get; set; }
    
    public Guid? TeamId { get; set; }

    public virtual DbTeam? Team { get; set; }

    public virtual List<DbTeamRolesToMembers>? TeamRolesToMembers { get; set; }

    public virtual List<DbMember>? TeamMembers { get; set; }
}

public enum DefaultMemberRoleType
{
    Командир = 1,
    Заместитель = 2,
    Рядовой = 3
}

internal sealed class DbTeamRolesMapping
{
    public void Map(EntityTypeBuilder<DbTeamRole> builder, Guid teamId, Dictionary<int, Guid> ids)
    {
        builder.ToTable("TeamRoles");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Rank).IsRequired().HasMaxLength(255);
        builder.HasOne(x => x.Team).WithMany(x => x.TeamRoles).HasForeignKey(c => c.TeamId);

        var roles = Enum.GetValues<DefaultMemberRoleType>().Select(v => new DbTeamRole
        {
            Id = ids[(int)v],
            Title = v.ToString(),
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            Rank = (int)v,
            TeamId = teamId
        })
            .ToArray();
        builder.HasData(roles);
    }
}