using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleDetail;

public class ProductSaleDetailRepository : EfEntityRepositoryBase<ProductSaleDetailEntity, TixappContext>, IProductSaleDetailRepository
{
    public ProductSaleDetailRepository(TixappContext context) : base(context)
    {
    }
}