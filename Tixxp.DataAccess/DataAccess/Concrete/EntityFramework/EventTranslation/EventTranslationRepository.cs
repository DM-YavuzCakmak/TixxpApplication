using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.EventTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.EventTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.EventTranslation;

public class EventTranslationRepository : EfEntityRepositoryBase<EventTranslationEntity, TixappContext>, IEventTranslationRepository
{
    public EventTranslationRepository(TixappContext context) : base(context)
    {
    }
}

