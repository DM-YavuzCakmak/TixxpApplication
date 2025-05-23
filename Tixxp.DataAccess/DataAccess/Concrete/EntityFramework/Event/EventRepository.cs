using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Events;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Event;

public class EventRepository : EfEntityRepositoryBase<EventEntity, TixappContext>, IEventRepository
{
    public EventRepository(TixappContext context) : base(context)
    {
    }
}
