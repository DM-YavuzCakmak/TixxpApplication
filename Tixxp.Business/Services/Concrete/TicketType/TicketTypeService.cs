using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketType;

namespace Tixxp.Business.Services.Concrete.TicketType;

public class TicketTypeService : BaseService<TicketTypeEntity>, ITicketTypeService
{
    private readonly ITicketTypeRepository _ticketTypeRepository;


    public TicketTypeService(ITicketTypeRepository ticketTypeRepository)
        : base(ticketTypeRepository)
    {
        _ticketTypeRepository = ticketTypeRepository;
    }
}
