using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketType;

namespace Tixxp.Infrastructure.DataAccess.Abstract.TicketType;

public interface ITicketTypeRepository : IEntityRepository<TicketTypeEntity>
{
}
