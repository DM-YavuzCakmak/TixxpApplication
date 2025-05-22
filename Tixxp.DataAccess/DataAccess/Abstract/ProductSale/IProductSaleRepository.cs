using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSale;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductSale
{
    public interface IProductSaleRepository : IEntityRepository<ProductSaleEntity>
    {
    }
}
