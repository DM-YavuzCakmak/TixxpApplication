using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SessionTypeTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Ticket;

namespace Tixxp.Business.Services.Concrete.SessionTypeTranslation;

public class SessionTypeTranslationService : BaseService<SessionTypeTranslationEntity>, ISessionTypeTranslationService
{
    private readonly ISessionTypeTranslationRepository _sessionTypeTranslationRepository;
    public SessionTypeTranslationService(ISessionTypeTranslationRepository sessionTypeTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(sessionTypeTranslationRepository, logService, httpContextAccessor)
    {
        _sessionTypeTranslationRepository = sessionTypeTranslationRepository;
    }
}
