using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Country;
using Tixxp.Business.Services.Abstract.County;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Country;
using Tixxp.Entities.County;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.County;

namespace Tixxp.Business.Services.Concrete.County;

public class CountyService : BaseService<CountyEntity>, ICountyService
{
    private readonly ICountyRepository _countyRepository;

    public CountyService(ICountyRepository countyRepository)
        : base(countyRepository)
    {
        _countyRepository = countyRepository;
    }
}
