using Tixxp.Business.Services.Abstract.ReservationDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationDetail;

namespace Tixxp.Business.Services.Concrete.ReservationDetail;

public class ReservationDetailService : BaseService<ReservationDetailEntity>, IReservationDetailService
{
    private readonly IReservationDetailRepository _reservationDetailRepository ;
    public ReservationDetailService(IReservationDetailRepository reservationDetailRepository)
        : base(reservationDetailRepository)
    {
        _reservationDetailRepository = reservationDetailRepository;
    }
}
