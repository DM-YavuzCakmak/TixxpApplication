using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Session;

public class SessionRepository : EfEntityRepositoryBase<SessionEntity, TixappContext>, ISessionRepository
{
    public SessionRepository(TixappContext context) : base(context)
    {
    }
}
