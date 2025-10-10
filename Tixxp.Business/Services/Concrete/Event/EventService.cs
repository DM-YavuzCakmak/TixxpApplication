using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Events;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.Event;

public class EventService : BaseService<EventEntity>, IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(eventRepository, logService, httpContextAccessor)
    {
        _eventRepository = eventRepository;
    }
}
