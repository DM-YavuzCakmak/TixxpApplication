using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.EventTicketPrice;
namespace Tixxp.Infrastructure.DataAccess.Abstract.EventTicketPrice;

public interface IEventTicketPriceRepository : IEntityRepository<EventTicketPriceEntity>
{
}
