using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationProductDetail;

public class ReservationProductDetailRepository : EfEntityRepositoryBase<ReservationProductDetailEntity, TixappContext>, IReservationProductDetailRepository
{
    public ReservationProductDetailRepository(TixappContext context) : base(context)
    {
    }
}