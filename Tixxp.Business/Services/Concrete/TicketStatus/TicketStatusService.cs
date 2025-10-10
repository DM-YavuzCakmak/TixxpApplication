using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.TicketStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatusTranslation;
namespace Tixxp.Business.Services.Concrete.TicketStatus;

public class TicketStatusService : BaseService<TicketStatusEntity>, ITicketStatusService
{
    private readonly ITicketStatusRepository _ticketStatusRepository;
    public TicketStatusService(ITicketStatusRepository ticketStatusRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(ticketStatusRepository, logService, httpContextAccessor)
    {
        _ticketStatusRepository = ticketStatusRepository;
    }
}
