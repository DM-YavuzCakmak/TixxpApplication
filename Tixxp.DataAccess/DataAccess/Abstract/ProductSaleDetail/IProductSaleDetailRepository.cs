using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ProductSaleDetail;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;

public interface IProductSaleDetailRepository : IEntityRepository<ProductSaleDetailEntity>
{
}
