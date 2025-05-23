using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Events;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;

namespace Tixxp.Business.Services.Concrete.Event;

public class EventService : BaseService<EventEntity>, IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
        : base(eventRepository)
    {
        _eventRepository = eventRepository;
    }
}
