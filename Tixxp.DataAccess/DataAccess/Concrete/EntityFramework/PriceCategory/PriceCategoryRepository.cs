using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PriceCategory;

public class PriceCategoryRepository : EfEntityRepositoryBase<PriceCategoryEntity, CommonDbContext>, IPriceCategoryRepository
{
    public PriceCategoryRepository(CommonDbContext context) : base(context)
    {
    }
}
