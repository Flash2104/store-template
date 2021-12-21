using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entity;
using Store.Service.Common;
using Store.Service.Exceptions;

namespace Store.Service.Repositories;

public class CitiesRepository
{
    protected readonly IDbContext? _context;
    protected readonly DbSet<DbRuCity>? _dbSet;

    public CitiesRepository(IDbContext context)
    {
        this._context = context;
        this._dbSet = context.Set<DbRuCity>();
    }

    public virtual async Task<List<DbRuCity>> ListAsync(
        Expression<Func<DbRuCity, bool>>? filter = null,
        Func<IQueryable<DbRuCity>, IOrderedQueryable<DbRuCity>>? orderBy = null)
    {
        IQueryable<DbRuCity>? query = _dbSet;
        if (query == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }
}