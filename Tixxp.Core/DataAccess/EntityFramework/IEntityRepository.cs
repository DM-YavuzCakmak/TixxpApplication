using System.Linq.Expressions;
using Tixxp.Core.Entities;

namespace Tixxp.Core.DataAccess.EntityFramework;

public interface IEntityRepository<T> where T : class, IEntity, new()
{
    IList<T> GetAll();
    T Get(Expression<Func<T, bool>> filter);
    IList<T> GetList(Expression<Func<T, bool>> filter = null);
    IList<T> GetListWithInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);
    T? GetFirstOrDefault(Expression<Func<T, bool>> filter);
    T? GetFirstOrDefaultWithInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    T AddAndReturn(T entity);

}
