using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Session;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Session;

public interface ISessionRepository : IEntityRepository<SessionEntity>
{
}
