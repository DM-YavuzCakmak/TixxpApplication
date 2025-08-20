using Tixxp.Business.Services.Abstract.County;
using Tixxp.Business.Services.Abstract.CountyTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Abstract.CountyTranslation;

namespace Tixxp.Business.Services.Concrete.CountyTranslation;

public class CountyTranslationService : BaseService<CountyTranslationEntity>, ICountyTranslationService
{
    private readonly ICountyTranslationRepository _countyTranslationRepository;

    public CountyTranslationService(ICountyTranslationRepository countyTranslationRepository)
        : base(countyTranslationRepository)
    {
        _countyTranslationRepository = countyTranslationRepository;
    }
}
