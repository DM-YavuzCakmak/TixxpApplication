using Tixxp.Business.Services.Abstract.TicketStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;
namespace Tixxp.Business.Services.Concrete.TicketStatus;

public class TicketStatusService : BaseService<TicketStatusEntity>, ITicketStatusService
{
    private readonly ITicketStatusRepository _ticketStatusRepository;
    public TicketStatusService(ITicketStatusRepository ticketStatusRepository)
        : base(ticketStatusRepository)
    {
        _ticketStatusRepository = ticketStatusRepository;
    }
}
