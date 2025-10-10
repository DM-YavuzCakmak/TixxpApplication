using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Country;
using Tixxp.Business.Services.Abstract.CountryTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Country;
using Tixxp.Entities.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.CountryTranslation;

public class CountryTranslationService : BaseService<CountryTranslationEntity>, ICountryTranslationService
{
    private readonly ICountryTranslationRepository _countryTranslationRepository;

    public CountryTranslationService(ICountryTranslationRepository countryTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(countryTranslationRepository, logService, httpContextAccessor)
    {
        _countryTranslationRepository = countryTranslationRepository;
    }
}