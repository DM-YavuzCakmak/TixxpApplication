using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Counter;
using Tixxp.Entities.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.Product;

public class ProductService : BaseService<ProductEntity>, IProductService
{
    private readonly IProductRepository _productRepository;


    public ProductService(IProductRepository productRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(productRepository, logService, httpContextAccessor)
    {
        _productRepository = productRepository;
    }
}
