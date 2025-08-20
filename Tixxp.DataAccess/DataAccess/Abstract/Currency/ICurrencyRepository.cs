using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Currency;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Currency;

public interface ICurrencyRepository : IEntityRepository<CurrencyEntity>
{
}
