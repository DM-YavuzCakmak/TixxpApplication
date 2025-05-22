using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;

namespace Tixxp.Business.Services.Concrete.ProductSaleDetail;

public class ProductSaleDetailService : BaseService<ProductSaleDetailEntity>, IProductSaleDetailService
{
    private readonly IProductSaleDetailRepository _productSaleDetailRepository;
    public ProductSaleDetailService(IProductSaleDetailRepository productSaleDetailRepository)
        : base(productSaleDetailRepository)
    {
        _productSaleDetailRepository = productSaleDetailRepository;
    }
}
