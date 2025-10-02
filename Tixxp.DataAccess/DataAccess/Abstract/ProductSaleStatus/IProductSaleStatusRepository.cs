using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleStatus;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatus;

public interface IProductSaleStatusRepository : IEntityRepository<ProductSaleStatusEntity>
{
}
