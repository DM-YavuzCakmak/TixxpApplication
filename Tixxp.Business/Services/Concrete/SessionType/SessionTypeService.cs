using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.SessionType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionType;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionTypeTranslation;

namespace Tixxp.Business.Services.Concrete.SessionType;

public class SessionTypeService : BaseService<SessionTypeEntity>, ISessionTypeService
{
    private readonly ISessionTypeRepository _sessionTypeRepository;
    public SessionTypeService(ISessionTypeRepository sessionTypeRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(sessionTypeRepository, logService, httpContextAccessor)
    {
        _sessionTypeRepository = sessionTypeRepository;
    }
}
