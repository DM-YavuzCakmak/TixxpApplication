using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductSaleStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ProductSaleStatusTranslation;

public class ProductSaleStatusTranslationService : BaseService<ProductSaleStatusTranslationEntity>, IProductSaleStatusTranslationService
{
    private readonly IProductSaleStatusTranslationRepository _productSaleStatusTranslationRepository;
    public ProductSaleStatusTranslationService(IProductSaleStatusTranslationRepository productSaleStatusTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productSaleStatusTranslationRepository, logService, httpContextAccessor)
    {
        _productSaleStatusTranslationRepository = productSaleStatusTranslationRepository;
    }
}
