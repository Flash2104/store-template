using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Entity;
using Store.Service.Common;
using Store.Service.Exceptions;

namespace Store.Service.Repositories;

public class GenericRepository<TEntity> where TEntity : class, IDbEntity
{
    protected readonly IDbContext? _context;
    protected readonly DbSet<TEntity>? _dbSet;

    public GenericRepository(IDbContext context)
    {
        this._context = context;
        this._dbSet = context.Set<TEntity>();
    }

    public virtual async Task<List<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity>? query = _dbSet;
        if (query == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }

        query = query.Where(x => x.Deleted == null);
        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>>? filter)
    {
        IQueryable<TEntity>? query = _dbSet;
        if (query == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        query = query.Where(x => x.Deleted == null);
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return await query.SingleOrDefaultAsync();
    }

    public virtual async Task<TEntity?> GetIncludeAsync<TEntityProperty>(
        Expression<Func<TEntity, bool>>? filter,
        Expression<Func<TEntity, TEntityProperty>>? includePropertyExpression = null)
    {
        IQueryable<TEntity>? query = _dbSet;
        if (query == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        query = query.Where(x => x.Deleted == null);
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (includePropertyExpression != null)
        {
                query = query.Include(includePropertyExpression);
        }
        return await query.SingleOrDefaultAsync();
    }

    public virtual void Insert(TEntity entity)
    {
        _dbSet!.Add(entity);
    }

    public virtual void Delete(object id)
    {
        TEntity? entityToDelete = _dbSet?.Find(id);
        if (entityToDelete == null)
        {
            return;
        }
        if (_context == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        entityToDelete.Deleted = DateTime.UtcNow.ToLongDateString();
        _context.Entry(entityToDelete).State = EntityState.Modified;
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        _dbSet?.Attach(entityToDelete);
        if (_context == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        entityToDelete.Deleted = DateTime.UtcNow.ToLongDateString();
        _context.Entry(entityToDelete).State = EntityState.Modified;
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        _dbSet?.Attach(entityToUpdate);
        if (_context == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}