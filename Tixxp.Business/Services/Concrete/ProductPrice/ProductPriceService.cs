using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Bank;
using Tixxp.Entities.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;

namespace Tixxp.Business.Services.Concrete.ProductPrice;

public class ProductPriceService : BaseService<ProductPriceEntity>, IProductPriceService
{
    private readonly IProductPriceRepository _productPriceRepository;
    public ProductPriceService(IProductPriceRepository productPriceRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productPriceRepository, logService, httpContextAccessor)
    {
        _productPriceRepository = productPriceRepository;
    }
}
