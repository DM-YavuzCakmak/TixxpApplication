using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;

namespace Tixxp.Business.Services.Concrete.ProductSaleInvoiceInfo;

public class ProductSaleInvoiceInfoService : BaseService<ProductSaleInvoiceInfoEntity>, IProductSaleInvoiceInfoService
{
    private readonly IProductSaleInvoiceInfoRepository _productSaleInvoiceInfoRepository;
    public ProductSaleInvoiceInfoService(IProductSaleInvoiceInfoRepository productSaleInvoiceInfoRepository)
        : base(productSaleInvoiceInfoRepository)
    {
        _productSaleInvoiceInfoRepository = productSaleInvoiceInfoRepository;
    }
}
