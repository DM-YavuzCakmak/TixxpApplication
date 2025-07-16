using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Reservation;
namespace Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
public interface IReservationRepository : IEntityRepository<ReservationEntity>
{

}
