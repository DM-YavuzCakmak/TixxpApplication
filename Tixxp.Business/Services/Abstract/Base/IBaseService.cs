﻿using System.Linq.Expressions;
using Tixxp.Core.Entities;
using Tixxp.Core.Utilities.Results.Abstract;

namespace Tixxp.Business.Services.Abstract.Base
{
    public interface IBaseService<T> where T : class, IEntity, new()
    {
        IDataResult<T> GetById(long id);
        IDataResult<List<T>> GetAll();
        IDataResult<T> GetFirstOrDefault(Expression<Func<T, bool>> filter);
        IResult Add(T entity);
        IResult AddRange(List<T> entities);
        IResult Update(T entity);
        IResult Delete(T entity);
        IDataResult<T> AddAndReturn(T entity);
        IDataResult<List<T>> GetList(Expression<Func<T, bool>> filter);
        IDataResult<List<T>> GetListWithInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

    }
}
