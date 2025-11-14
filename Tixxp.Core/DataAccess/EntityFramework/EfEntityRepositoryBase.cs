using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tixxp.Core.Entities;

namespace Tixxp.Core.DataAccess.EntityFramework;

public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
    where TContext : DbContext
{
    protected readonly TContext _context;

    public EfEntityRepositoryBase(TContext context)
    {
        _context = context;
    }

    public void Add(TEntity entity)
    {
        entity.Created_Date = DateTime.Now;
        entity.IsDeleted = false; // yeni kayıtlar aktif olur
        _context.Entry(entity).State = EntityState.Added;
        _context.SaveChanges();
    }

    public void AddRange(IEnumerable<TEntity> entities)
    {
        var now = DateTime.Now;
        foreach (var entity in entities)
        {
            entity.Created_Date = now;
            entity.IsDeleted = false;
        }

        _context.Set<TEntity>().AddRange(entities);
        _context.SaveChanges();
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.Updated_Date = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public TEntity Get(Expression<Func<TEntity, bool>> filter)
    {
        return _context.Set<TEntity>().Where(x => x.IsDeleted == false).SingleOrDefault(filter);
    }

    public IList<TEntity> GetAll()
    {
        return _context.Set<TEntity>().Where(x => !x.IsDeleted).ToList();
    }

    public IList<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
    {
        var query = _context.Set<TEntity>().Where(x => !x.IsDeleted);
        return filter == null ? query.ToList() : query.Where(filter).ToList();
    }

    public IList<TEntity> GetListWithInclude(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().Where(x => !x.IsDeleted);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query.Where(filter).ToList();
    }

    public TEntity? GetFirstOrDefault(Expression<Func<TEntity, bool>> filter)
    {
        return _context.Set<TEntity>().Where(x => !x.IsDeleted).FirstOrDefault(filter);
    }

    public void Update(TEntity entity)
    {
        entity.Updated_Date = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public TEntity AddAndReturn(TEntity entity)
    {
        entity.Created_Date = DateTime.Now;
        entity.IsDeleted = false;
        _context.Set<TEntity>().Add(entity);
        _context.SaveChanges();
        return entity; // => burada Id dahil tüm değerleriyle geri döner
    }

    public TEntity? GetFirstOrDefaultWithInclude(
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return (filter != null)
            ? query.AsNoTracking().FirstOrDefault(filter)
            : query.AsNoTracking().FirstOrDefault();
    }
}
