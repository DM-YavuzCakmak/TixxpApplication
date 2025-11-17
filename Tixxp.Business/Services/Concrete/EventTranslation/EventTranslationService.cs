using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.EventTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.EventTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.EventTranslation;

namespace Tixxp.Business.Services.Concrete.EventTranslation;

public class EventTranslationService : BaseService<EventTranslationEntity>, IEventTranslationService
{
    private readonly IEventTranslationRepository _eventTranslationRepository;

    public EventTranslationService(IEventTranslationRepository eventTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(eventTranslationRepository, logService, httpContextAccessor)
    {
        _eventTranslationRepository = eventTranslationRepository;
    }
}

