using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SeasonalPrice;
public class SeasonalPriceRepository : EfEntityRepositoryBase<SeasonalPriceEntity, TixappContext>, ISeasonalPriceRepository
{
    public SeasonalPriceRepository(TixappContext context) : base(context)
    {
    }
}
