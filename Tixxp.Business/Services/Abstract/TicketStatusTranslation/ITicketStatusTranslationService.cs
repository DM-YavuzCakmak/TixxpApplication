using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Entities.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatusTranslation;

namespace Tixxp.Business.Services.Abstract.TicketStatusTranslation;

public interface ITicketStatusTranslationService : IBaseService<TicketStatusTranslationEntity>
{
}
