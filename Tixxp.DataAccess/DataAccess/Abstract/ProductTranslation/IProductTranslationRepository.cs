using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;

public interface IProductTranslationRepository : IEntityRepository<ProductTranslationEntity>
{
}
