using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationStatus;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;

public interface IReservationStatusRepository : IEntityRepository<ReservationStatusEntity>
{
}
