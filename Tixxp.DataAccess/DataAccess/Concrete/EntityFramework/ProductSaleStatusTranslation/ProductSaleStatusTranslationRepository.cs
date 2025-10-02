using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Entities.ProductSaleStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleStatusTranslation;

public class ProductSaleStatusTranslationRepository : EfEntityRepositoryBase<ProductSaleStatusTranslationEntity, TixappContext>, IProductSaleStatusTranslationRepository
{
    public ProductSaleStatusTranslationRepository(TixappContext context) : base(context)
    {
    }
}