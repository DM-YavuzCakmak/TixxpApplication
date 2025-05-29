using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PriceCategory;

namespace Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;

public interface IPriceCategoryRepository : IEntityRepository<PriceCategoryEntity>
{
}
