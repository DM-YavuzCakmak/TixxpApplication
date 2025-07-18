using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;

namespace Tixxp.Infrastructure.DataAccess.Abstract.SessionEventTicketPrice;

public interface ISessionEventTicketPriceRepository : IEntityRepository<SessionEventTicketPriceEntity>
{
}
