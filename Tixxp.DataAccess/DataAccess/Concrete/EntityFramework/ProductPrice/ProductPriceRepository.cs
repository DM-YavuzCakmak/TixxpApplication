using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Counter;
using Tixxp.Entities.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductPrice;

public class ProductPriceRepository : EfEntityRepositoryBase<ProductPriceEntity, TixappContext>, IProductPriceRepository
{
    public ProductPriceRepository(TixappContext context) : base(context)
    {
    }
}
