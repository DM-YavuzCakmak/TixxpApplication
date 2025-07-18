using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionEventTicketPrice;

public class SessionEventTicketPriceRepository : EfEntityRepositoryBase<SessionEventTicketPriceEntity, TixappContext>, ISessionEventTicketPriceRepository
{
    public SessionEventTicketPriceRepository(TixappContext context) : base(context)
    {
    }
}
