using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.BankService;

public class BankService : BaseService<BankEntity>, IBankService
{
    private readonly IBankRepository _bankRepository;


    public BankService(IBankRepository bankRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(bankRepository, logService, httpContextAccessor)
    {
        _bankRepository = bankRepository;
    }
}
