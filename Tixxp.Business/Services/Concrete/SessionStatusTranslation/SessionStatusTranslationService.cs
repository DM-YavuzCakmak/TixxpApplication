using Tixxp.Business.Services.Abstract.SessionStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.SessionStatusTranslation;

public class SessionStatusTranslationService : BaseService<SessionStatusTranslationEntity>, ISessionStatusTranslationService
{
    private readonly ISessionStatusTranslationRepository _sessionStatusTranslationRepository;
    public SessionStatusTranslationService(ISessionStatusTranslationRepository sessionStatusTranslationRepository)
        : base(sessionStatusTranslationRepository)
    {
        _sessionStatusTranslationRepository = sessionStatusTranslationRepository;
    }
}
