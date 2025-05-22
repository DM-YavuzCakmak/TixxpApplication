using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSale;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSale;

namespace Tixxp.Business.Services.Concrete.ProductSale;

public class ProductSaleService : BaseService<ProductSaleEntity>, IProductSaleService
{
    private readonly IProductSaleRepository _productSaleRepository;
    public ProductSaleService(IProductSaleRepository productSaleRepository)
        : base(productSaleRepository)
    {
        _productSaleRepository = productSaleRepository;
    }
}
