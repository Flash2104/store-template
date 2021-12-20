
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeamRolesToMembers
{
    public Guid MemberId { get; set; }
    public virtual DbMember? Member { get; set; }

    public Guid TeamRoleId { get; set; }
    public virtual DbTeamRole? Role { get; set; }
}

internal sealed class DbTeamRolesToMembersMapping
{
    public void Map(EntityTypeBuilder<DbTeamRolesToMembers> builder)
    {
        builder.ToTable("TeamRolesToMembers");

        builder.HasKey(x => new { x.MemberId, x.TeamRoleId });
        builder
             .HasOne(x => x.Member)
             .WithMany(x => x.TeamRolesToMembers)
             .HasForeignKey(j => j.MemberId);
        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.TeamRolesToMembers)
            .HasForeignKey(j => j.TeamRoleId);
    }
}
