using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.SessionStatusTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;

public interface ISessionStatusTranslationRepository : IEntityRepository<SessionStatusTranslationEntity>
{
}
