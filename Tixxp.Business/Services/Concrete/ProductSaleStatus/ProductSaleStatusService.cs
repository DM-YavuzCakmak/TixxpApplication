using Tixxp.Business.Services.Abstract.ProductSaleStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatus;

namespace Tixxp.Business.Services.Concrete.ProductSaleStatus;

public class ProductSaleStatusService : BaseService<ProductSaleStatusEntity>, IProductSaleStatusService
{
    private readonly IProductSaleStatusRepository _productSaleStatusRepository;
    public ProductSaleStatusService(IProductSaleStatusRepository productSaleStatusRepository)
        : base(productSaleStatusRepository)
    {
        _productSaleStatusRepository = productSaleStatusRepository;
    }
}
