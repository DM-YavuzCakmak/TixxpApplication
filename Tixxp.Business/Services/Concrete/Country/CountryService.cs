using Tixxp.Business.Services.Abstract.Country;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;

namespace Tixxp.Business.Services.Concrete.Country;

public class CountryService : BaseService<CountryEntity>, ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
        : base(countryRepository)
    {
        _countryRepository = countryRepository;
    }
}
