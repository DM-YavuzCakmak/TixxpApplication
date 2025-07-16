using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;
using Tixxp.Infrastructure.DataAccess.Abstract.EventTicketPrice;

namespace Tixxp.Business.Services.Concrete.EventTicketPrice;

public class EventTicketPriceService : BaseService<EventTicketPriceEntity>, IEventTicketPriceService
{
    private readonly IEventTicketPriceRepository _eventTicketPriceRepository;

    public EventTicketPriceService(IEventTicketPriceRepository eventTicketPriceRepository)
        : base(eventTicketPriceRepository)
    {
        _eventTicketPriceRepository = eventTicketPriceRepository;
    }
}
