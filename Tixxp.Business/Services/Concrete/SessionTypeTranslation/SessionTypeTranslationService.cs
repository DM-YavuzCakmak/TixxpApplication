using Tixxp.Business.Services.Abstract.SessionTypeTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;

namespace Tixxp.Business.Services.Concrete.SessionTypeTranslation;

public class SessionTypeTranslationService : BaseService<SessionTypeTranslationEntity>, ISessionTypeTranslationService
{
    private readonly ISessionTypeTranslationRepository _sessionTypeTranslationRepository;
    public SessionTypeTranslationService(ISessionTypeTranslationRepository sessionTypeTranslationRepository)
        : base(sessionTypeTranslationRepository)
    {
        _sessionTypeTranslationRepository = sessionTypeTranslationRepository;
    }
}
