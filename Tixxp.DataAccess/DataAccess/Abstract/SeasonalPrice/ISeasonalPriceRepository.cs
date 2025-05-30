using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SeasonalPrice;
namespace Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;

public interface ISeasonalPriceRepository : IEntityRepository<SeasonalPriceEntity>
{
}
