using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;

namespace Tixxp.Business.Services.Concrete.Session;

public class SessionService : BaseService<SessionEntity>, ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    public SessionService(ISessionRepository sessionRepository)
        : base(sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
}
