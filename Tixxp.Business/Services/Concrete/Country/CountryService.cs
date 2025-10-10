using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Country;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.Country;

public class CountryService : BaseService<CountryEntity>, ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(countryRepository, logService, httpContextAccessor)
    {
        _countryRepository = countryRepository;
    }
}
