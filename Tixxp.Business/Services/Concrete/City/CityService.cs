using Tixxp.Business.Services.Abstract.City;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.City;
using Tixxp.Infrastructure.DataAccess.Abstract.City;

namespace Tixxp.Business.Services.Concrete.City;

public class CityService : BaseService<CityEntity>, ICityService
{
    private readonly ICityRepository _cityRepository;
    public CityService(ICityRepository cityRepository)
        : base(cityRepository)
    {
        _cityRepository = cityRepository;
    }
}
