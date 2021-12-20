using System.Linq.Expressions;
using AirSoft.Data;
using AirSoft.Data.Entity;
using AirSoft.Service.Common;
using AirSoft.Service.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AirSoft.Service.Repositories;

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