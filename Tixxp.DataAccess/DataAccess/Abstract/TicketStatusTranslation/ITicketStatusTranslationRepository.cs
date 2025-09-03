using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.TicketStatusTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.TicketStatusTranslation;

public interface ITicketStatusTranslationRepository : IEntityRepository<TicketStatusTranslationEntity>
{
}
