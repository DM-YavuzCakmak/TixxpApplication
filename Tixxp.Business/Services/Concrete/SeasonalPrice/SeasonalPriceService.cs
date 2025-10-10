using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SeasonalPrice;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.SeasonalPrice;

public class SeasonalPriceService : BaseService<SeasonalPriceEntity>, ISeasonalPriceService
{
    private readonly ISeasonalPriceRepository _seasonalPriceRepository;
    public SeasonalPriceService(ISeasonalPriceRepository seasonalPriceRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(seasonalPriceRepository, logService, httpContextAccessor)
    {
        _seasonalPriceRepository = seasonalPriceRepository;
    }
}
