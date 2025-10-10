using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.PriceCategory;

public class PriceCategoryService : BaseService<PriceCategoryEntity>, IPriceCategoryService
{
    private readonly IPriceCategoryRepository _priceCategoryRepository;

    public PriceCategoryService(IPriceCategoryRepository priceCategoryRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(priceCategoryRepository, logService, httpContextAccessor)
    {
        _priceCategoryRepository = priceCategoryRepository;
    }
}
