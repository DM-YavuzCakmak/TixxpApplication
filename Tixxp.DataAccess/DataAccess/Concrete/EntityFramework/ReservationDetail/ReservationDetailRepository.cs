using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationDetail;

public class ReservationDetailRepository : EfEntityRepositoryBase<ReservationDetailEntity, TixappContext>, IReservationDetailRepository
{
    public ReservationDetailRepository(TixappContext context) : base(context)
    {
    }
}