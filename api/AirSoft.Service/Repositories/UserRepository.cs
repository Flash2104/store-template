using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entity;
using Store.Service.Common;
using Store.Service.Exceptions;

namespace Store.Service.Repositories;

public class UserRepository : GenericRepository<DbUser>
{
    public UserRepository(IDbContext context) : base(context)
    {
    }

    public async Task<DbUser?> GetByPhoneAsync(string phone)
    {
        var users = await ListAsync(e => e.Phone == phone);
        var dbUsers = users.ToList();
        if (dbUsers.Count > 1)
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.MoreThanOneUserByPhone,
                "В базе больше одного пользователя по данному номеру телефона.", "");
        }

        return dbUsers.FirstOrDefault();
    }

    public async Task<DbUser?> GetByEmailAsync(string email)
    {
        var users = await ListAsync(e => e.Email == email);
        var dbUsers = users.ToList();
        if (dbUsers.Count > 1)
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.MoreThanOneUserByEmail,
                "В базе больше одного пользователя по данному email.", "");
        }

        return dbUsers.FirstOrDefault();
    }

    public DbUser CreateNewUser(DbUser user)
    {
        return Insert(user);
    }
    
    public async Task AddUserRole(Guid userId, DbUserRole userRole)
    {
        DbUser? dbUser = await GetAsync(x => x.Id == userId);

        if (dbUser == null)
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.UserNotFound, "Пользователь не найден");
        }

        if (dbUser.UserRoles != null && dbUser.UserRoles.Any(x => x.Id == userRole.Id))
        {
            throw new AirSoftBaseException(ErrorCodes.UserRepository.UserAlreadyHaveRole, $"У пользователя уже есть роль: {userRole.Role}");
        }
        
        dbUser.UsersToRoles = new List<DbUsersToRoles>()
        {
            new()
            {
                UserId = dbUser.Id,
                RoleId = userRole.Id
            }
        };
    }
}