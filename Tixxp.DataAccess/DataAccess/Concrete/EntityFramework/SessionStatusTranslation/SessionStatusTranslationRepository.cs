using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

public class SessionStatusTranslationRepository : EfEntityRepositoryBase<SessionStatusTranslationEntity, TixappContext>, ISessionStatusTranslationRepository
{
    public SessionStatusTranslationRepository(TixappContext context) : base(context)
    {
    }
}
