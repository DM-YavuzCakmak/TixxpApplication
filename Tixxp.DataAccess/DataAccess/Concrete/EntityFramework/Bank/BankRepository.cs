using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

public class BankRepository : EfEntityRepositoryBase<BankEntity, TixappContext>, IBankRepository
{
    public BankRepository(TixappContext context) : base(context)
    {
    }
}
