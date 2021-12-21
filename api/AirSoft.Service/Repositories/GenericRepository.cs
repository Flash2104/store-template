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
            return await orderBy(query).SingleOrDefaultAsync();
        }
        return await query.SingleOrDefaultAsync();
    }

    public virtual TEntity Insert(TEntity entity)
    {
        return _dbSet!.Add(entity).Entity;
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

        //Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        //if (_context?.Entry(entityToDelete).State == EntityState.Detached)
        //{
        //    _dbSet?.Attach(entityToDelete);
        //}
        _dbSet?.Attach(entityToDelete);
        if (_context == null)
        {
            throw new AirSoftBaseException(ErrorCodes.CommonError, "Пустой датасет.");
        }
        entityToDelete.Deleted = DateTime.UtcNow.ToLongDateString();
        _context.Entry(entityToDelete).State = EntityState.Modified;
        //_dbSet?.Remove(entityToDelete);
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