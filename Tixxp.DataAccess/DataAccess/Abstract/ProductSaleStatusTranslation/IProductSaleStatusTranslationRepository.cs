using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Entities.ProductSaleStatusTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatusTranslation;

public interface IProductSaleStatusTranslationRepository : IEntityRepository<ProductSaleStatusTranslationEntity>
{
}
