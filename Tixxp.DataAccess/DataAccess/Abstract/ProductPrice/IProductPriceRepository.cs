using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductPrice;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;

public interface IProductPriceRepository : IEntityRepository<ProductPriceEntity>
{
}
