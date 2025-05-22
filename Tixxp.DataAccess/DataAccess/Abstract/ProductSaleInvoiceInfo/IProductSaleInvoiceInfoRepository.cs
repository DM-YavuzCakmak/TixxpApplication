using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleInvoiceInfo;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;

public interface IProductSaleInvoiceInfoRepository : IEntityRepository<ProductSaleInvoiceInfoEntity>
{
}
