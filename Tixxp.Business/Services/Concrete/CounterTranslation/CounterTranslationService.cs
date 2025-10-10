using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.CounterTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CounterTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CounterTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.CounterTranslation;

public class CounterTranslationService : BaseService<CounterTranslationEntity>, ICounterTranslationService
{
    private readonly ICounterTranslationRepository _counterTranslationRepository;
    public CounterTranslationService(ICounterTranslationRepository counterTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(counterTranslationRepository, logService, httpContextAccessor)
    {
        _counterTranslationRepository = counterTranslationRepository;
    }
}
