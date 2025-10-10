using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.TicketSubType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketType;

namespace Tixxp.Business.Services.Concrete.TicketSubType;

public class TicketSubTypeService : BaseService<TicketSubTypeEntity>, ITicketSubTypeService
{
    private readonly ITicketSubTypeRepository _ticketSubTypeRepository;
    public TicketSubTypeService(
        ITicketSubTypeRepository ticketSubTypeRepository,
        ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(ticketSubTypeRepository, logService, httpContextAccessor)
    {
        _ticketSubTypeRepository = ticketSubTypeRepository;
    }
}