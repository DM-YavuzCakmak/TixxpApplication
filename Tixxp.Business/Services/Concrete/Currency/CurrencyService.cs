using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.Currency;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Counter;
using Tixxp.Entities.Currency;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Currency;

namespace Tixxp.Business.Services.Concrete.Currency;

public class CurrencyService : BaseService<CurrencyEntity>, ICurrencyService
{
    private readonly ICurrencyRepository _currencyRepository;

    public CurrencyService(ICurrencyRepository currencyRepository)
        : base(currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }
}
