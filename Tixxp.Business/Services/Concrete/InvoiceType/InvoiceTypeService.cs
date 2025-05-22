using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;

namespace Tixxp.Business.Services.Concrete.InvoiceType;

public class InvoiceTypeService : BaseService<InvoiceTypeEntity>, IInvoiceTypeService
{
    private readonly IInvoiceTypeRepository _invoceTypeRepository;


    public InvoiceTypeService(IInvoiceTypeRepository invoiceTypeRepository)
        : base(invoiceTypeRepository)
    {
        _invoceTypeRepository = invoiceTypeRepository;
    }
}
