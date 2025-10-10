using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ProductTranslation;

public class ProductTranslationService : BaseService<ProductTranslationEntity>, IProductTranslationService
{
    private readonly IProductTranslationRepository _productTranslationRepository;
    public ProductTranslationService(IProductTranslationRepository productTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productTranslationRepository, logService, httpContextAccessor)
    {
        _productTranslationRepository = productTranslationRepository;
    }
}
