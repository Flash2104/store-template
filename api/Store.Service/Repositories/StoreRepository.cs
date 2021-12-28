using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entity;
using Store.Service.Common;
using Store.Service.Exceptions;

namespace Store.Service.Repositories;

public class StoreRepository
{
    private readonly IDbContext? _context;
    private readonly DbSet<DbStore>? _dbSet;

    public StoreRepository(IDbContext context)
    {
        this._context = context;
        this._dbSet = context.Set<DbStore>();
    }

    public virtual async Task<DbStore?> GetAsync()
    {
        IQueryable<DbStore>? query = _dbSet;
        if (query == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет магазина.");
        }
        query = query.Where(x => x.Deleted == null);

        return await query.SingleOrDefaultAsync(x => x.Id == 1);
    }
    public virtual void Update(DbStore entityToUpdate)
    {
        if (_dbSet == null || _context == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет магазина.");
        }
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}