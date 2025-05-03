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

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.Updated_Date = DateTime.Now;
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public TEntity Get(Expression<Func<TEntity, bool>> filter)
    {
        return _context.Set<TEntity>().Where(x => !x.IsDeleted).SingleOrDefault(filter);
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
}
