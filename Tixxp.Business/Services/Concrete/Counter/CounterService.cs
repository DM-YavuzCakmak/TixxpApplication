using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.Counter;

public class CounterService : BaseService<CounterEntity>, ICounterService
{
    private readonly ICounterRepository _counterRepository;


    public CounterService(ICounterRepository counterRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(counterRepository, logService, httpContextAccessor)
    {
        _counterRepository = counterRepository;
    }
}
