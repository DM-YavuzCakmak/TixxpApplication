using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionType;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionType;

public class SessionTypeRepository : EfEntityRepositoryBase<SessionTypeEntity, TixappContext>, ISessionTypeRepository
{
    public SessionTypeRepository(TixappContext context) : base(context)
    {
    }
}
