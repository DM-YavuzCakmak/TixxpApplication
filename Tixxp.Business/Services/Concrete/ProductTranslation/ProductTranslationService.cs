using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;

namespace Tixxp.Business.Services.Concrete.ProductTranslation;

public class ProductTranslationService : BaseService<ProductTranslationEntity>, IProductTranslationService
{
    private readonly IProductTranslationRepository _productTranslationRepository;
    public ProductTranslationService(IProductTranslationRepository productTranslationRepository)
        : base(productTranslationRepository)
    {
        _productTranslationRepository = productTranslationRepository;
    }
}
