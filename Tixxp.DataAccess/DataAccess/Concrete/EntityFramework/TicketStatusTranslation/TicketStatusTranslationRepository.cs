using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatusTranslation;

public class TicketStatusTranslationRepository : EfEntityRepositoryBase<TicketStatusTranslationEntity, TixappContext>, ITicketStatusTranslationRepository
{
    public TicketStatusTranslationRepository(TixappContext context) : base(context)
    {
    }
}