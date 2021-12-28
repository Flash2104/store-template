using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Store.Data.Entity;

public class DbUser: DbEntity<int>
{
    public DbUser()
    {
    }

    private DbUser(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    private ILazyLoader LazyLoader { get; set; } = null!;

    private List<DbUserRole>? _userRoles;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte[]? AvatarIcon { get; set; }

    public byte[]? Avatar { get; set; }

    public UserStatus Status { get; set; }

    public string? PasswordHash { get; set; }

    public List<DbUserRole>? UserRoles
    {
        get => LazyLoader.Load(this, ref _userRoles);
        set => _userRoles = value;
    }

    public virtual List<DbUsersToRoles>? UsersToRoles { get; set; }

    public string HashPassword(string password)
    {
        var ph = new PasswordHasher<DbUser>();
        return ph.HashPassword(this, password);
    }

    public bool ValidPassword(string password)
    {
        var ph = new PasswordHasher<DbUser>();
        var result = ph.VerifyHashedPassword(this, PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}

public enum UserStatus
{
    None = 0, 
    New = 1,
    Confirmed = 2
}

internal sealed class DbUserMapping
{
    public void Map(EntityTypeBuilder<DbUser> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(x => x.CreatedDate).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ModifiedDate).IsRequired().HasMaxLength(50);

        var converter = new ValueConverter<UserStatus, string>(
            v => v.ToString(),
            v => (UserStatus)Enum.Parse(typeof(UserStatus), v));

        builder
            .Property(e => e.Status)
            .IsRequired()
            .HasConversion(converter);
        string? root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var admin = new DbUser
        {
            Id = 1,
            CreatedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            ModifiedDate = new DateTime(2021, 12, 02, 1, 50, 00),
            CreatedBy = 1,
            ModifiedBy = 1,
            Email = "khoruzhenko.work@gmail.com",
            PasswordHash = "AQAAAAEAACcQAAAAEMQnvSxDqgyc+KNNzIFjcuST/qZGfHVSLT9P+Z3revJP2Q9Tctz8PIeDxj2k7aJkLg==",
            Phone = "89266762453",
            Status = UserStatus.Confirmed,
            Avatar = File.ReadAllBytes(root + "\\InitialData\\admin.png"),
            AvatarIcon = File.ReadAllBytes(root + "\\InitialData\\admin-icon.png")
        };
        builder.HasData(admin);
        builder.HasMany(x => x.UserRoles).WithMany(x => x.Users).UsingEntity<DbUsersToRoles>(
            x => x.HasData(new List<DbUsersToRoles>()
            {
                new DbUsersToRoles()
                {
                    UserId = 1,
                    RoleId = (int) UserRoleType.Administrator
                }
            })
            );
    }
}
