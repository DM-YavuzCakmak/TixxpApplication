using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionTypeTranslation;
using Tixxp.Entities.Ticket;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Ticket;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Ticket;

public class TicketRepository : EfEntityRepositoryBase<TicketEntity, TixappContext>, ITicketRepository
{
    public TicketRepository(TixappContext context) : base(context)
    {
    }
}