using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatus;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleStatus;

public class ProductSaleStatusRepository : EfEntityRepositoryBase<ProductSaleStatusEntity, TixappContext>, IProductSaleStatusRepository
{
    public ProductSaleStatusRepository(TixappContext context) : base(context)
    {
    }
}