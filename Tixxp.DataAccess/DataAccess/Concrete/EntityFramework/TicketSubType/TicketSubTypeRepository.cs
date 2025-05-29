using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketSubType;

public class TicketSubTypeRepository : EfEntityRepositoryBase<TicketSubTypeEntity, TixappContext>, ITicketSubTypeRepository
{
    public TicketSubTypeRepository(TixappContext context) : base(context)
    {
    }
}
