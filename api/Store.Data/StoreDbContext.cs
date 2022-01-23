using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Store.Data.Entity;
using Store.Data.Entity.Category;
using Store.Data.Entity.Product;

namespace Store.Data;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    Task SaveAsync();

    public void Initialize();
}

public class StoreDbContext : DbContext, IDbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor = null!;

    public StoreDbContext()
    {

    }

    public StoreDbContext(DbContextOptions<StoreDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public async Task SaveAsync()
    {
        await base.SaveChangesAsync();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        var entities = ChangeTracker.Entries().Where(x => x.Entity is IDbEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        var userId = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var currentUsername = !string.IsNullOrEmpty(userId)
            ? int.Parse(userId)
            : 0;

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
        
        new DbRuCityMapping().Map(modelBuilder.Entity<DbRuCity>()); // Список ру городов по регионам

        new DbUserRolesMapping().Map(modelBuilder.Entity<DbUserRole>());
        new DbUsersToRolesMapping().Map(modelBuilder.Entity<DbUsersToRoles>());
        new DbUserMapping().Map(modelBuilder.Entity<DbUser>());

        new DbStoreMapping().Map(modelBuilder.Entity<DbStore>());
        new DbCategoryTreeMapping().Map(modelBuilder.Entity<DbCategoryTree>());
        new DbCategoryItemMapping().Map(modelBuilder.Entity<DbCategoryItem>());
        new DbProductMapping().Map(modelBuilder.Entity<DbProduct>());
        new DbProductImageMapping().Map(modelBuilder.Entity<DbProductImage>());
        new DbProductsToCategoriesMapping().Map(modelBuilder.Entity<DbProductsToCategories>());
    }
}
