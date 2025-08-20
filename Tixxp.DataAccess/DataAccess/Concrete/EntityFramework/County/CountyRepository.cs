using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Country;
using Tixxp.Entities.County;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

public class CountyRepository : EfEntityRepositoryBase<CountyEntity, CommonDbContext>, ICountyRepository
{
    public CountyRepository(CommonDbContext context) : base(context)
    {
    }
}
