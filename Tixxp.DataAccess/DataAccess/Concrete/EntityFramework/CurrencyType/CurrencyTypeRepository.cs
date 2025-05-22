using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType;

public class CurrencyTypeRepository : EfEntityRepositoryBase<CurrencyTypeEntity, CommonDbContext>, ICurrencyTypeRepository
{
    public CurrencyTypeRepository(CommonDbContext context) : base(context)
    {
    }
}