using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Store.Data.Entity;

public class DbUserNavigation: DbEntity<Guid>
{
    public DbUserNavigation()
    {
    }
    private DbUserNavigation(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; } = null!;

    private List<DbNavigationItem>? _navigationItems;

    public string Title { get; set; } = null!;

    public bool IsDefault { get; set; }

    public Guid? UserId { get; set; }

    public virtual DbUser? User { get; set; }

    public virtual List<DbNavigationItem>? NavigationItems
    {
        get => LazyLoader.Load(this, ref _navigationItems);
        set => _navigationItems = value;
    }

    public virtual List<DbNavigationsToNavigationItems>? NavigationsToNavigationItems { get; set; }
}

internal sealed class DbUserNavigationMapping
{
    public void Map(EntityTypeBuilder<DbUserNavigation> builder, Guid userId, Dictionary<int, Guid> roleNavIds)
    {
        builder.ToTable("UserNavigations");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
        builder.Property(x => x.IsDefault).IsRequired();
        builder
            .HasOne(x => x.User)
            .WithMany(x => x.UserNavigations)
            .HasForeignKey(x => x.UserId );

        builder.HasData(new List<DbUserNavigation>()
        {
            new DbUserNavigation()
            {
                Id = roleNavIds[(int)UserRoleType.TeamManager],
                Title = "Навигация менеджера команды",
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                IsDefault = true
            },
            new DbUserNavigation()
            {
                Id = roleNavIds[(int)UserRoleType.Player],
                Title = "Навигация Игрока",
                ModifiedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                IsDefault = false
            }
        });
    }
}