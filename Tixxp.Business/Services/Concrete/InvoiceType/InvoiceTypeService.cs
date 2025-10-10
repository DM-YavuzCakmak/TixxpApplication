using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.InvoiceType;

public class InvoiceTypeService : BaseService<InvoiceTypeEntity>, IInvoiceTypeService
{
    private readonly IInvoiceTypeRepository _invoceTypeRepository;


    public InvoiceTypeService(IInvoiceTypeRepository invoiceTypeRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(invoiceTypeRepository, logService, httpContextAccessor)
    {
        _invoceTypeRepository = invoiceTypeRepository;
    }
}
