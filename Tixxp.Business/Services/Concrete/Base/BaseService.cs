using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Core.Entities;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;

namespace Tixxp.Business.Services.Concrete.Base;

public class BaseService<T> : IBaseService<T> where T : class, IEntity, new()
{
    private readonly IEntityRepository<T> _repository;

    public BaseService(IEntityRepository<T> repository)
    {
        _repository = repository;
    }

    public IDataResult<T> GetById(long id)
    {
        var result = _repository.Get(x => x.Id == id);
        if (result != null)
            return new SuccessDataResult<T>(result);
        return new ErrorDataResult<T>("Kayıt bulunamadı.");
    }

    public IDataResult<List<T>> GetAll()
    {
        var list = _repository.GetAll().ToList();
        return new SuccessDataResult<List<T>>(list);
    }

    public IResult Add(T entity)
    {
        _repository.Add(entity);
        return new SuccessResult("Kayıt eklendi.");
    }

    public IResult Update(T entity)
    {
        _repository.Update(entity);
        return new SuccessResult("Kayıt güncellendi.");
    }

    public IResult Delete(T entity)
    {
        _repository.Delete(entity);
        return new SuccessResult("Kayıt silindi.");
    }

    public IDataResult<T> AddAndReturn(T entity)
    {
        var result = _repository.AddAndReturn(entity);
        return new SuccessDataResult<T>(result, "Kayıt başarıyla eklendi.");
    }
}
