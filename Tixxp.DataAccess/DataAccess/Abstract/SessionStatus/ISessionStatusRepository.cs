using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionStatus;
namespace Tixxp.Infrastructure.DataAccess.Abstract.SessionStatus;

public interface ISessionStatusRepository : IEntityRepository<SessionStatusEntity>
{
}
