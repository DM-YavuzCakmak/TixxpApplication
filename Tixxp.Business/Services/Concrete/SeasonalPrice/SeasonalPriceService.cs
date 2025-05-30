using Tixxp.Business.Services.Abstract.SeasonalPrice;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;

namespace Tixxp.Business.Services.Concrete.SeasonalPrice;

public class SeasonalPriceService : BaseService<SeasonalPriceEntity>, ISeasonalPriceService
{
    private readonly ISeasonalPriceRepository _seasonalPriceRepository;
    public SeasonalPriceService(ISeasonalPriceRepository seasonalPriceRepository)
        : base(seasonalPriceRepository)
    {
        _seasonalPriceRepository = seasonalPriceRepository;
    }
}
