using Tixxp.Business.Services.Abstract.ProductSaleStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ProductSaleStatusTranslation;

public class ProductSaleStatusTranslationService : BaseService<ProductSaleStatusTranslationEntity>, IProductSaleStatusTranslationService
{
    private readonly IProductSaleStatusTranslationRepository _productSaleStatusTranslationRepository;
    public ProductSaleStatusTranslationService(IProductSaleStatusTranslationRepository productSaleStatusTranslationRepository)
        : base(productSaleStatusTranslationRepository)
    {
        _productSaleStatusTranslationRepository = productSaleStatusTranslationRepository;
    }
}
