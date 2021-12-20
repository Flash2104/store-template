using System.Security.Claims;
using AirSoft.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AirSoft.Data;

public interface IDbContext : IDisposable
{
    DbSet<DbUser>? Users { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task SaveAsync();

    public void Initialize();
}

public class AirSoftDbContext : DbContext, IDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = null!;

    public AirSoftDbContext()
    {

    }

    public AirSoftDbContext(DbContextOptions<AirSoftDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public DbSet<DbUser>? Users { get; set; }

    public async Task SaveAsync()
    {
        await base.SaveChangesAsync();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        var entities = ChangeTracker.Entries().Where(x => x.Entity is IDbEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var userId = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var currentUsername = !string.IsNullOrEmpty(userId)
            ? Guid.Parse(userId)
            : Guid.Empty;

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                ((IDbEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                ((IDbEntity)entity.Entity).CreatedBy = currentUsername;
            }
            ((IDbEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
            ((IDbEntity)entity.Entity).ModifiedBy = currentUsername;
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public void Initialize()
    {
        try
        {
            this.Database.Migrate();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Data context initialization failed.", ex);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        var userId = Guid.Parse("fadde9ec-7dc4-4033-b1e6-2f83a08c70f3");
        var memberId = Guid.Parse("703405e5-9cc1-434e-8c18-d19bb7fbd9f2");
        var teamId = Guid.Parse("be955114-80ca-4b6c-9295-022c2460d48f");
        var teamRoleIds = new Dictionary<int, Guid>(Enum.GetValues<DefaultMemberRoleType>().Select(x => new KeyValuePair<int, Guid>((int)x, Guid.NewGuid())));
        var roleNavIds = new Dictionary<int, Guid>(Enum.GetValues<UserRoleType>()
            .Where(x => x != UserRoleType.None)
            .Select(x => new KeyValuePair<int, Guid>((int)x, Guid.NewGuid())));

        
        new DbRuCityMapping().Map(modelBuilder.Entity<DbRuCity>()); // Список ру городов по регионам

        new DbUserRolesMapping().Map(modelBuilder.Entity<DbUserRole>());
        new DbUsersToRolesMapping().Map(modelBuilder.Entity<DbUsersToRoles>());
        new DbUserMapping().Map(modelBuilder.Entity<DbUser>(), userId);

        new DbMemberMapping().Map(modelBuilder.Entity<DbMember>(), userId, memberId, teamId, teamRoleIds);
        new DbTeamMapping().Map(modelBuilder.Entity<DbTeam>(), userId, teamId, memberId);
        new DbTeamRolesMapping().Map(modelBuilder.Entity<DbTeamRole>(), teamId, teamRoleIds);
        new DbTeamRolesToMembersMapping().Map(modelBuilder.Entity<DbTeamRolesToMembers>());

        new DbUserNavigationMapping().Map(modelBuilder.Entity<DbUserNavigation>(), userId, roleNavIds);
        new DbNavigationItemsMapping().Map(modelBuilder.Entity<DbNavigationItem>(), roleNavIds);
        new DbUserRolesToNavigationItemsMapping().Map(modelBuilder.Entity<DbUserRolesToNavigationItems>());
        new DbNavigationsToNavigationItemsMapping().Map(modelBuilder.Entity<DbNavigationsToNavigationItems>());
    }
}
