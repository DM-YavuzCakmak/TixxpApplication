using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ProductSale;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSale;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.ProductSale;

public class ProductSaleService : BaseService<ProductSaleEntity>, IProductSaleService
{
    private readonly IProductSaleRepository _productSaleRepository;
    public ProductSaleService(IProductSaleRepository productSaleRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productSaleRepository, logService, httpContextAccessor)
    {
        _productSaleRepository = productSaleRepository;
    }
}
