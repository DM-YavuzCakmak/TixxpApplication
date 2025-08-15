using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionTypeTranslation;

public class SessionTypeTranslationRepository : EfEntityRepositoryBase<SessionTypeTranslationEntity, TixappContext>, ISessionTypeTranslationRepository
{
    public SessionTypeTranslationRepository(TixappContext context) : base(context)
    {
    }
}