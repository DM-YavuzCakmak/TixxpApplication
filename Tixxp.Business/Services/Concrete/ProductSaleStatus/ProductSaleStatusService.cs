using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductSaleStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.ProductSaleStatus;

public class ProductSaleStatusService : BaseService<ProductSaleStatusEntity>, IProductSaleStatusService
{
    private readonly IProductSaleStatusRepository _productSaleStatusRepository;
    public ProductSaleStatusService(IProductSaleStatusRepository productSaleStatusRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productSaleStatusRepository, logService, httpContextAccessor)
    {
        _productSaleStatusRepository = productSaleStatusRepository;
    }
}
