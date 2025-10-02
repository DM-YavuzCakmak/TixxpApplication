using Tixxp.Business.Services.Abstract.CounterTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CounterTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.CounterTranslation;

public class CounterTranslationService : BaseService<CounterTranslationEntity>, ICounterTranslationService
{
    private readonly ICounterTranslationRepository _counterTranslationRepository;
    public CounterTranslationService(ICounterTranslationRepository counterTranslationRepository)
        : base(counterTranslationRepository)
    {
        _counterTranslationRepository = counterTranslationRepository;
    }
}
