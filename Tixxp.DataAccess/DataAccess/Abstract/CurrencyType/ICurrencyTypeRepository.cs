using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CurrencyType;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;

public interface ICurrencyTypeRepository : IEntityRepository<CurrencyTypeEntity>
{
}
