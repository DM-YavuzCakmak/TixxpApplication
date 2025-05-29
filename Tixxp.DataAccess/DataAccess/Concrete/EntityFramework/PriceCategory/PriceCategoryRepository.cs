using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PriceCategory;

public class PriceCategoryRepository : EfEntityRepositoryBase<PriceCategoryEntity, TixappContext>, IPriceCategoryRepository
{
    public PriceCategoryRepository(TixappContext context) : base(context)
    {
    }
}
