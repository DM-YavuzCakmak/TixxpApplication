using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Country;

public class CountryRepository : EfEntityRepositoryBase<CountryEntity, CommonDbContext>, ICountryRepository
{
    public CountryRepository(CommonDbContext context) : base(context)
    {
    }
}
