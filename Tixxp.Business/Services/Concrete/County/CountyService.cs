using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Country;
using Tixxp.Business.Services.Abstract.County;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Country;
using Tixxp.Entities.County;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

namespace Tixxp.Business.Services.Concrete.County;

public class CountyService : BaseService<CountyEntity>, ICountyService
{
    private readonly ICountyRepository _countyRepository;

    public CountyService(ICountyRepository countyRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(countyRepository, logService, httpContextAccessor)
    {
        _countyRepository = countyRepository;
    }
}
