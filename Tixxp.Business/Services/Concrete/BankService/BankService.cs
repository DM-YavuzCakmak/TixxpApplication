using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;

namespace Tixxp.Business.Services.Concrete.BankService;

public class BankService : BaseService<BankEntity>, IBankService
{
    private readonly IBankRepository _bankRepository;


    public BankService(IBankRepository bankRepository)
        : base(bankRepository)
    {
        _bankRepository = bankRepository;
    }
}
