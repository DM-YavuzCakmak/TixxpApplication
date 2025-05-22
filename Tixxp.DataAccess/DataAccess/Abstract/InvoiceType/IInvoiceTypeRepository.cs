using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.InvoiceType;

namespace Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;

public interface IInvoiceTypeRepository : IEntityRepository<InvoiceTypeEntity>
{
}
