
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbMember : DbEntity<Guid>
{
    public DbMember()
    {
    }

    private DbMember(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    private List<DbTeamRole>? _teamMemberRoles;
    private DbTeam? _team;
    private DbUser? _user;

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public int? CityAddressId { get; set; }

    public virtual DbRuCity? CityAddress { get; set; }

    public DateTime? BirthDate { get; set; }

    public byte[]? Avatar { get; set; }

    public byte[]? AvatarIcon { get; set; }

    public Guid? TeamId { get; set; }

    public string? About { get; set; }

    public DbTeam? Team
    {
        get => LazyLoader.Load(this, ref _team);
        set => _team = value;
    }

    public Guid? UserId { get; set; }

    public DbUser? User
    {
        get => LazyLoader.Load(this, ref _user);
        set => _user = value;
    }

    public List<DbTeamRole>? TeamMemberRoles
    {
        get => LazyLoader.Load(this, ref _teamMemberRoles);
        set => _teamMemberRoles = value;
    }

    public virtual List<DbTeamRolesToMembers>? TeamRolesToMembers { get; set; }
}

internal sealed class DbMemberMapping
{
    public void Map(EntityTypeBuilder<DbMember> builder, Guid userId, Guid memberId, Guid teamId, Dictionary<int, Guid> teamRoleIds)
    {
        builder.ToTable("Members");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Surname).HasMaxLength(255);
        builder.Property(x => x.Avatar);

        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);

        builder.HasOne(x => x.Team).WithMany(x => x.Members).HasForeignKey(x => x.TeamId);
        builder.HasOne(x => x.CityAddress).WithMany().HasForeignKey(x => x.CityAddressId);
        builder.HasOne(x => x.User).WithOne(x => x.Member).HasForeignKey<DbMember>(x => x.UserId);
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var adminProfile = new DbMember()
        {
            Id = memberId,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = userId,
            ModifiedBy = userId,
            Name = "Кирилл",
            Surname = "Хоруженко",
            BirthDate = new DateTime(1993, 4, 21),
            CityAddressId = 1,
            TeamId = teamId,
            UserId = userId,
            Avatar = File.ReadAllBytes(root + "\\InitialData\\admin.png"),
            AvatarIcon = File.ReadAllBytes(root + "\\InitialData\\admin-icon.png"),
            About = "Создатель приложения"
        };
        builder.HasData(adminProfile);
        builder.HasMany(x => x.TeamMemberRoles).WithMany(x => x.TeamMembers).UsingEntity<DbTeamRolesToMembers>(
            x => x.HasData(new List<DbTeamRolesToMembers>()
            {
                new DbTeamRolesToMembers()
                {
                    MemberId = memberId,
                    TeamRoleId = teamRoleIds[(int)DefaultMemberRoleType.Командир]
                },
                new DbTeamRolesToMembers()
                {
                    MemberId = memberId,
                    TeamRoleId = teamRoleIds[(int)DefaultMemberRoleType.Рядовой]
                },
            }));
    }
}