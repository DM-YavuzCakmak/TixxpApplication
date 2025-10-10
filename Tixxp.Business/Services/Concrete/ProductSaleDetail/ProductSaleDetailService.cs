using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.ProductSaleDetail;

public class ProductSaleDetailService : BaseService<ProductSaleDetailEntity>, IProductSaleDetailService
{
    private readonly IProductSaleDetailRepository _productSaleDetailRepository;
    public ProductSaleDetailService(IProductSaleDetailRepository productSaleDetailRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productSaleDetailRepository, logService, httpContextAccessor)
    {
        _productSaleDetailRepository = productSaleDetailRepository;
    }
}
