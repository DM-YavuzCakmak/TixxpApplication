using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;

namespace Tixxp.Business.Services.Concrete.PriceCategory;

public class PriceCategoryService : BaseService<PriceCategoryEntity>, IPriceCategoryService
{
    private readonly IPriceCategoryRepository _priceCategoryRepository;

    public PriceCategoryService(IPriceCategoryRepository priceCategoryRepository)
        : base(priceCategoryRepository)
    {
        _priceCategoryRepository = priceCategoryRepository;
    }
}
