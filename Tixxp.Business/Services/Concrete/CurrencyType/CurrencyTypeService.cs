using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
namespace Tixxp.Business.Services.Concrete.CurrencyType;

public class CurrencyTypeService : BaseService<CurrencyTypeEntity>, ICurrencyTypeService
{
    private readonly ICurrencyTypeRepository _currencyTypeRepository;

    public CurrencyTypeService(ICurrencyTypeRepository currencyTypeRepository)
     : base(currencyTypeRepository)
    {
        _currencyTypeRepository = currencyTypeRepository;
    }
}
