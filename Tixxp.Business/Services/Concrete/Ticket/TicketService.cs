using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SessionType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionType;
using Tixxp.Entities.Ticket;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionType;
using Tixxp.Infrastructure.DataAccess.Abstract.Ticket;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatus;

namespace Tixxp.Business.Services.Concrete.Ticket;

public class TicketService : BaseService<TicketEntity>, ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    public TicketService(ITicketRepository ticketRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(ticketRepository, logService, httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
    }
}
