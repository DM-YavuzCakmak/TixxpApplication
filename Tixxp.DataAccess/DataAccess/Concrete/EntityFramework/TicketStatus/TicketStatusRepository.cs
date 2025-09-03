using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Ticket;
using Tixxp.Entities.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.Ticket;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatus;

public class TicketStatusRepository : EfEntityRepositoryBase<TicketStatusEntity, TixappContext>, ITicketStatusRepository
{
    public TicketStatusRepository(TixappContext context) : base(context)
    {
    }
}