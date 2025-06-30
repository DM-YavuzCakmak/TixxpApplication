using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Bank;
using Tixxp.Entities.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.InvoiceType;
public class InvoiceTypeRepository : EfEntityRepositoryBase<InvoiceTypeEntity, TixappContext>, IInvoiceTypeRepository
{
    public InvoiceTypeRepository(TixappContext context) : base(context)
    {
    }
}
