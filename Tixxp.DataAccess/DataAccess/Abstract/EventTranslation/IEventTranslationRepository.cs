using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.EventTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.EventTranslation;

public interface IEventTranslationRepository : IEntityRepository<EventTranslationEntity>
{
}

