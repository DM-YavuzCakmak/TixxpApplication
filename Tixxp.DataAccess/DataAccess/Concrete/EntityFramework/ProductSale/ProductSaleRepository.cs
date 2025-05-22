using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSale;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSale;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSale;

public class ProductSaleRepository : EfEntityRepositoryBase<ProductSaleEntity, TixappContext>, IProductSaleRepository
{
    public ProductSaleRepository(TixappContext context) : base(context)
    {
    }
}
