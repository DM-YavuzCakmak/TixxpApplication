using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SessionStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionType;

namespace Tixxp.Business.Services.Concrete.SessionStatusTranslation;

public class SessionStatusTranslationService : BaseService<SessionStatusTranslationEntity>, ISessionStatusTranslationService
{
    private readonly ISessionStatusTranslationRepository _sessionStatusTranslationRepository;
    public SessionStatusTranslationService(ISessionStatusTranslationRepository sessionStatusTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(sessionStatusTranslationRepository, logService, httpContextAccessor)
    {
        _sessionStatusTranslationRepository = sessionStatusTranslationRepository;
    }
}
