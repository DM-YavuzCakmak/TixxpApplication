using Tixxp.Business.Services.Abstract.ReservationProductDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationProductDetail;

namespace Tixxp.Business.Services.Concrete.ReservationProductDetail;

public class ReservationProductDetailService : BaseService<ReservationProductDetailEntity>, IReservationProductDetailService
{
    private readonly IReservationProductDetailRepository _reservationProductDetailRepository;
    public ReservationProductDetailService(IReservationProductDetailRepository reservationProductDetailRepository)
        : base(reservationProductDetailRepository)
    {
        _reservationProductDetailRepository = reservationProductDetailRepository;
    }
}