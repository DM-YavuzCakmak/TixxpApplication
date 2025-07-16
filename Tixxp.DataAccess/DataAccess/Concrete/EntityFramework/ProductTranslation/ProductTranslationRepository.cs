using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductTranslation;

public class ProductTranslationRepository : EfEntityRepositoryBase<ProductTranslationEntity, TixappContext>, IProductTranslationRepository
{
    public ProductTranslationRepository(TixappContext context) : base(context)
    {
    }
}