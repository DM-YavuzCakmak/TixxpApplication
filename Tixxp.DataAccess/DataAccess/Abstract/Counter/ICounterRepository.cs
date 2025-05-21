using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Counter;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Counter;

public interface ICounterRepository : IEntityRepository<CounterEntity>
{
}
