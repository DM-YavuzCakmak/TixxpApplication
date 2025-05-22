using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Product;

public class ProductRepository : EfEntityRepositoryBase<ProductEntity, TixappContext>, IProductRepository
{
    public ProductRepository(TixappContext context) : base(context)
    {
    }
}