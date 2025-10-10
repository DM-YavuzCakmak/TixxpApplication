using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.County;
using Tixxp.Business.Services.Abstract.CountyTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Abstract.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.CountyTranslation;

public class CountyTranslationService : BaseService<CountyTranslationEntity>, ICountyTranslationService
{
    private readonly ICountyTranslationRepository _countyTranslationRepository;

    public CountyTranslationService(ICountyTranslationRepository countyTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(countyTranslationRepository, logService, httpContextAccessor)
    {
        _countyTranslationRepository = countyTranslationRepository;
    }
}
