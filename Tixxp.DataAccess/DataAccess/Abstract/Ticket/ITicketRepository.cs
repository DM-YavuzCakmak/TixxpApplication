using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Ticket;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Ticket;

public interface ITicketRepository : IEntityRepository<TicketEntity>
{
}
