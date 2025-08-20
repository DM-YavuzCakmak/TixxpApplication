using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationStatus;

public class ReservationStatusRepository : EfEntityRepositoryBase<ReservationStatusEntity, TixappContext>, IReservationStatusRepository
{
    public ReservationStatusRepository(TixappContext context) : base(context)
    {
    }
}
