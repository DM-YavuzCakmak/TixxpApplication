using Tixxp.Business.Services.Abstract.SessionStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatus;

namespace Tixxp.Business.Services.Concrete.SessionStatus;

public class SessionStatusService : BaseService<SessionStatusEntity>, ISessionStatusService
{
    private readonly ISessionStatusRepository _sessionStatusRepository;
    public SessionStatusService(ISessionStatusRepository sessionStatusRepository)
        : base(sessionStatusRepository)
    {
        _sessionStatusRepository = sessionStatusRepository;
    }
}
