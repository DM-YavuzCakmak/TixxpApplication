using Tixxp.Business.Services.Abstract.TicketSubType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketSubType;

namespace Tixxp.Business.Services.Concrete.TicketSubType;

public class TicketSubTypeService : BaseService<TicketSubTypeEntity>, ITicketSubTypeService
{
    private readonly ITicketSubTypeRepository _ticketSubTypeRepository;
    public TicketSubTypeService(ITicketSubTypeRepository ticketSubTypeRepository)
        : base(ticketSubTypeRepository)
    {
        _ticketSubTypeRepository = ticketSubTypeRepository;
    }
}