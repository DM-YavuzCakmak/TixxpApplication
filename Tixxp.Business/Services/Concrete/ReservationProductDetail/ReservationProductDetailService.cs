using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ReservationProductDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ReservationProductDetail;

public class ReservationProductDetailService : BaseService<ReservationProductDetailEntity>, IReservationProductDetailService
{
    private readonly IReservationProductDetailRepository _reservationProductDetailRepository;
    public ReservationProductDetailService(IReservationProductDetailRepository reservationProductDetailRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(reservationProductDetailRepository, logService, httpContextAccessor)
    {
        _reservationProductDetailRepository = reservationProductDetailRepository;
    }
}