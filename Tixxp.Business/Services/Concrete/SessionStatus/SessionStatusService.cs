using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SessionStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.SessionStatus;

public class SessionStatusService : BaseService<SessionStatusEntity>, ISessionStatusService
{
    private readonly ISessionStatusRepository _sessionStatusRepository;
    public SessionStatusService(ISessionStatusRepository sessionStatusRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(sessionStatusRepository, logService, httpContextAccessor)
    {
        _sessionStatusRepository = sessionStatusRepository;
    }
}
