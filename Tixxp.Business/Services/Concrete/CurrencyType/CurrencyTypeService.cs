using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;
namespace Tixxp.Business.Services.Concrete.CurrencyType;

public class CurrencyTypeService : BaseService<CurrencyTypeEntity>, ICurrencyTypeService
{
    private readonly ICurrencyTypeRepository _currencyTypeRepository;

    public CurrencyTypeService(ICurrencyTypeRepository currencyTypeRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(currencyTypeRepository, logService, httpContextAccessor)
    {
        _currencyTypeRepository = currencyTypeRepository;
    }
}
