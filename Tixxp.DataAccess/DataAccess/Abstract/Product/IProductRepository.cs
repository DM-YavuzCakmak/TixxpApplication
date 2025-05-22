using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Product;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Product;

public interface IProductRepository : IEntityRepository<ProductEntity>
{
}
