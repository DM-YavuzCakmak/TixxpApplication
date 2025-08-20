using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Currency;
using Tixxp.Infrastructure.DataAccess.Abstract.Currency;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Currency;

public class CurrencyRepository : EfEntityRepositoryBase<CurrencyEntity, TixappContext>, ICurrencyRepository
{
    public CurrencyRepository(TixappContext context) : base(context)
    {
    }
}
