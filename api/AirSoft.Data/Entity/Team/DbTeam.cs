using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirSoft.Data.Entity;

public class DbTeam : DbEntity<Guid>
{
    public DbTeam()
    {
    }
    private DbTeam(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    private List<DbMember>? _members;
    private List<DbTeamRole>? _teamRoles;

    public string Title { get; set; } = null!;
    
    public Guid LeaderId { get; set; }

    public DateTime? FoundationDate { get; set; }

    public int? CityAddressId { get; set; }

    public virtual DbRuCity? CityAddress { get; set; }

    public byte[]? Avatar { get; set; }

    public List<DbMember>? Members
    {
        get => LazyLoader.Load(this, ref _members);
        set => _members = value;
    }

    public virtual List<DbTeamRole>? TeamRoles
    {
        get => LazyLoader.Load(this, ref _teamRoles);
        set => _teamRoles = value;
    }

    public DbMember? Leader => Members?.FirstOrDefault(x => x.Id == LeaderId);
}

internal sealed class DbTeamMapping
{
    public void Map(EntityTypeBuilder<DbTeam> builder, Guid userId, Guid teamId, Guid memberId)
    {
        builder.ToTable("Teams");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.CreatedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedBy).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LeaderId).IsRequired().HasMaxLength(50);
        builder.HasOne(x => x.CityAddress).WithMany().HasForeignKey(x => x.CityAddressId);

        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var team = new DbTeam
        {
            Id = teamId,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = userId,
            ModifiedBy = userId,
            Title = "AirSoft Events",
            CityAddressId = 1,
            LeaderId = memberId,
            FoundationDate = new DateTime(2021, 12, 02, 1, 50, 00),
            Avatar = File.ReadAllBytes(root + "\\InitialData\\team.png")
        };
        builder.HasData(team);
    }
}
