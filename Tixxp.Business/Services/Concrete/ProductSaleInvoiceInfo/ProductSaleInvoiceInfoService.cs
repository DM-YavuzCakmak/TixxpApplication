using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.ProductSaleInvoiceInfo;

public class ProductSaleInvoiceInfoService : BaseService<ProductSaleInvoiceInfoEntity>, IProductSaleInvoiceInfoService
{
    private readonly IProductSaleInvoiceInfoRepository _productSaleInvoiceInfoRepository;
    public ProductSaleInvoiceInfoService(IProductSaleInvoiceInfoRepository productSaleInvoiceInfoRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productSaleInvoiceInfoRepository, logService, httpContextAccessor)
    {
        _productSaleInvoiceInfoRepository = productSaleInvoiceInfoRepository;
    }
}
