using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.CityTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.CityTranslation;

public class CityTranslationService : BaseService<CityTranslationEntity>, ICityTranslationService
{
    private readonly ICityTranslationRepository _cityTranslationRepository;
    public CityTranslationService(ICityTranslationRepository cityTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(cityTranslationRepository, logService, httpContextAccessor)
    {
        _cityTranslationRepository = cityTranslationRepository;
    }
}

