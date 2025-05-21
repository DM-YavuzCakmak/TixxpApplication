using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Counter;

public class CounterRepository : EfEntityRepositoryBase<CounterEntity, TixappContext>, ICounterRepository
{
    public CounterRepository(TixappContext context) : base(context)
    {
    }
}
