using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketType;

public class TicketTypeRepository : EfEntityRepositoryBase<TicketTypeEntity, TixappContext>, ITicketTypeRepository
{
    public TicketTypeRepository(TixappContext context) : base(context)
    {
    }
}
