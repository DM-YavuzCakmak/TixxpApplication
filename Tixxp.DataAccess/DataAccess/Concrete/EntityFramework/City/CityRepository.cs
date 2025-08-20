using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Channel;
using Tixxp.Entities.City;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.City;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.City;

public class CityRepository : EfEntityRepositoryBase<CityEntity, CommonDbContext>, ICityRepository
{
    public CityRepository(CommonDbContext context) : base(context)
    {
    }
}