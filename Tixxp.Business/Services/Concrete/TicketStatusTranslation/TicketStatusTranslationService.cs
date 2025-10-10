using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.TicketStatus;
using Tixxp.Business.Services.Abstract.TicketStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketStatus;
using Tixxp.Entities.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketSubType;

namespace Tixxp.Business.Services.Concrete.TicketStatusTranslation;

public class TicketStatusTranslationService : BaseService<TicketStatusTranslationEntity>, ITicketStatusTranslationService
{
    private readonly ITicketStatusTranslationRepository _ticketStatusTranslationRepository;
    public TicketStatusTranslationService(ITicketStatusTranslationRepository ticketStatusTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(ticketStatusTranslationRepository, logService, httpContextAccessor)
    {
        _ticketStatusTranslationRepository = ticketStatusTranslationRepository;
    }
}
