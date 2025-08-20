using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Country;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Country;

public interface ICountryRepository : IEntityRepository<CountryEntity>
{
}
