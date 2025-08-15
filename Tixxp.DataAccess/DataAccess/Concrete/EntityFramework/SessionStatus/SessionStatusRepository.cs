using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Context;
namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatus;

public class SessionStatusRepository : EfEntityRepositoryBase<SessionStatusEntity, TixappContext>, ISessionStatusRepository
{
    public SessionStatusRepository(TixappContext context) : base(context)
    {
    }
}
