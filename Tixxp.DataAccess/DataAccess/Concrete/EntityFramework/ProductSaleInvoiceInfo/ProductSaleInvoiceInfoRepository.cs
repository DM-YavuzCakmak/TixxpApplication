using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleInvoiceInfo;

public class ProductSaleInvoiceInfoRepository : EfEntityRepositoryBase<ProductSaleInvoiceInfoEntity, TixappContext>, IProductSaleInvoiceInfoRepository
{
    public ProductSaleInvoiceInfoRepository(TixappContext context) : base(context)
    {
    }
}