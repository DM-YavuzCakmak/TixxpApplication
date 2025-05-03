using Tixxp.Core.Entities;
using Tixxp.Core.Utilities.Results.Abstract;

namespace Tixxp.Business.Services.Abstract.Base
{
    public interface IBaseService<T> where T : class, IEntity, new()
    {
        IDataResult<T> GetById(long id);
        IDataResult<List<T>> GetAll();
        IResult Add(T entity);
        IResult Update(T entity);
        IResult Delete(T entity);
    }
}
