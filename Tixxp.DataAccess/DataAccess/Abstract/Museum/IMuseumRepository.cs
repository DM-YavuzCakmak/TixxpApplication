using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.Museum;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Museum;

public interface IMuseumRepository : IEntityRepository<MuseumEntity>
{
}
