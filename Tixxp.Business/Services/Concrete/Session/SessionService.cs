using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.Session;

public class SessionService : BaseService<SessionEntity>, ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    public SessionService(ISessionRepository sessionRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(sessionRepository, logService, httpContextAccessor)
    {
        _sessionRepository = sessionRepository;
    }
}
