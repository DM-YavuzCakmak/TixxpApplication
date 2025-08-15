using Tixxp.Business.Services.Abstract.SessionType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionType;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionType;

namespace Tixxp.Business.Services.Concrete.SessionType;

public class SessionTypeService : BaseService<SessionTypeEntity>, ISessionTypeService
{
    private readonly ISessionTypeRepository _sessionTypeRepository;
    public SessionTypeService(ISessionTypeRepository sessionTypeRepository)
        : base(sessionTypeRepository)
    {
        _sessionTypeRepository = sessionTypeRepository;
    }
}
