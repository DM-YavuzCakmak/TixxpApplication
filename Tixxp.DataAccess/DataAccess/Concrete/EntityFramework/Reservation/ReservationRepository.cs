using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Reservation;

public class ReservationRepository : EfEntityRepositoryBase<ReservationEntity, TixappContext>, IReservationRepository
{
    public ReservationRepository(TixappContext context) : base(context)
    {
    }
}