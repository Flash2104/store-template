using Store.Data;
using Store.Data.Entity;

namespace Store.Service.Repositories;

public class MemberRepository: GenericRepository<DbMember>
{
    public MemberRepository(IDbContext context) : base(context)
    {
    }

    public async Task<DbMember?> GetByUserAsync(Guid userId)
    {
        var dbMember = await GetAsync(e => e.UserId == userId, null, "CityAddress");
        return dbMember;
    }
}