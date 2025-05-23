using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Events;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Event;

public interface IEventRepository : IEntityRepository<EventEntity>
{
}
