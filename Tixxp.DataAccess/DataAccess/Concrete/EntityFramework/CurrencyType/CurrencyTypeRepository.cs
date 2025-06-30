using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType;

public class CurrencyTypeRepository : EfEntityRepositoryBase<CurrencyTypeEntity, TixappContext>, ICurrencyTypeRepository
{
    public CurrencyTypeRepository(TixappContext context) : base(context)
    {
    }
}