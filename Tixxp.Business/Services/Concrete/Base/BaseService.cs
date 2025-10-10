using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using System.Text.Json;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Core.Entities;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Core.Utilities.Results.Concrete;

namespace Tixxp.Business.Services.Concrete.Base
{
    public class BaseService<T> : IBaseService<T> where T : class, IEntity, new()
    {
        private readonly IEntityRepository<T> _repository;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(IEntityRepository<T> repository, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
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

            var logModel = CreateBaseLogModel(entity, null, entity, "Add");
            logModel.IsFirst = true;
            _ = _logService.CreateHistoryLogAsync(logModel);

            return new SuccessResult("Kayıt eklendi.");
        }

        public IResult AddRange(List<T> entities)
        {
            _repository.AddRange(entities);

            var logModel = new HistoryLogModel
            {
                CorrelationId = Guid.NewGuid(),
                EntityName = typeof(T).Name,
                ProjectName = "Tixxp",
                MethodName = "AddRange",
                IsFirst = false,
                NewData = JsonSerializer.Serialize(entities),
                UserInformation = GetCurrentUserInfo(),
                CreatedDate = DateTime.UtcNow,
                RequestDate = DateTime.UtcNow
            };

            _ = _logService.CreateHistoryLogAsync(logModel);

            return new SuccessResult("Kayıtlar başarıyla eklendi.");
        }

        public IResult Update(T entity)
        {
            var oldEntity = _repository.Get(x => x.Id == entity.Id);
            _repository.Update(entity);

            var logModel = CreateBaseLogModel(entity, oldEntity, entity, "Update");
            _ = _logService.CreateHistoryLogAsync(logModel);

            return new SuccessResult("Kayıt güncellendi.");
        }

        public IResult Delete(T entity)
        {
            var oldEntity = _repository.Get(x => x.Id == entity.Id);
            _repository.Delete(entity);

            var logModel = CreateBaseLogModel(entity, oldEntity, entity, "Delete");
            _ = _logService.CreateHistoryLogAsync(logModel);

            return new SuccessResult("Kayıt silindi.");
        }

        public IDataResult<T> AddAndReturn(T entity)
        {
            var result = _repository.AddAndReturn(entity);

            var logModel = CreateBaseLogModel(result, null, result, "AddAndReturn");
            logModel.IsFirst = true;
            _ = _logService.CreateHistoryLogAsync(logModel);

            return new SuccessDataResult<T>(result, "Kayıt başarıyla eklendi.");
        }

        public IDataResult<List<T>> GetList(Expression<Func<T, bool>> filter)
        {
            var list = _repository.GetList(filter).ToList();
            return new SuccessDataResult<List<T>>(list);
        }

        public IDataResult<List<T>> GetListWithInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            var list = _repository.GetListWithInclude(filter, includes).ToList();
            return new SuccessDataResult<List<T>>(list);
        }

        public IDataResult<T> GetFirstOrDefault(Expression<Func<T, bool>> filter)
        {
            var result = _repository.GetFirstOrDefault(filter);
            if (result != null)
                return new SuccessDataResult<T>(result);
            return new ErrorDataResult<T>("Kayıt bulunamadı.");
        }

        public IDataResult<T> GetFirstOrDefaultWithInclude(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            includes ??= Array.Empty<Expression<Func<T, object>>>();
            var result = _repository.GetFirstOrDefaultWithInclude(filter, includes);
            if (result != null)
                return new SuccessDataResult<T>(result);
            return new ErrorDataResult<T>("Kayıt bulunamadı.");
        }

        // 🔹 Ortak log modeli oluşturma helper metodu
        private HistoryLogModel CreateBaseLogModel(T entity, T? oldEntity, T? newEntity, string methodName)
        {
            return new HistoryLogModel
            {
                EntityId = entity.Id.ToString(),
                CorrelationId = Guid.NewGuid(),
                ProjectName = "Tixxp",
                MethodName = methodName,
                EntityName = typeof(T).Name,
                IsFirst = false,
                OldData = oldEntity != null ? JsonSerializer.Serialize(oldEntity) : null,
                NewData = newEntity != null ? JsonSerializer.Serialize(newEntity) : null,
                UserInformation = GetCurrentUserInfo(),
                CreatedDate = DateTime.UtcNow,
                RequestDate = DateTime.UtcNow
            };
        }

        // 🔹 Kullanıcı bilgisi (HttpContext'ten)
        private string GetCurrentUserInfo()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var name = user.Identity.Name;
                return !string.IsNullOrEmpty(name) ? name : "AuthenticatedUser";
            }
            return "Anonymous";
        }
    }
}
