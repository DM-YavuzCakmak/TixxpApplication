using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.City;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.City;
using Tixxp.Infrastructure.DataAccess.Abstract.City;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.City;

public class CityService : BaseService<CityEntity>, ICityService
{
    private readonly ICityRepository _cityRepository;
    public CityService(ICityRepository cityRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(cityRepository, logService, httpContextAccessor)
    {
        _cityRepository = cityRepository;
    }
}
