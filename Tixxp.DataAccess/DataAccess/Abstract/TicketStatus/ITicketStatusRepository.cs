using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketStatus;

namespace Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;

public interface ITicketStatusRepository : IEntityRepository<TicketStatusEntity>
{
}
