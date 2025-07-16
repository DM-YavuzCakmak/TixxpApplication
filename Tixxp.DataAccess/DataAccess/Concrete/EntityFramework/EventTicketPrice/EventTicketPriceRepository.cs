using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.EventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.EventTicketPrice;

public class EventTicketPriceRepository : EfEntityRepositoryBase<EventTicketPriceEntity, TixappContext>, IEventTicketPriceRepository
{
    public EventTicketPriceRepository(TixappContext context) : base(context)
    {
    }
}
