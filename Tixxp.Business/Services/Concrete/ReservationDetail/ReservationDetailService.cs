using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.ReservationDetail;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ReservationDetail;

public class ReservationDetailService : BaseService<ReservationDetailEntity>, IReservationDetailService
{
    private readonly IReservationDetailRepository _reservationDetailRepository ;
    public ReservationDetailService(IReservationDetailRepository reservationDetailRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(reservationDetailRepository, logService, httpContextAccessor)
    {
        _reservationDetailRepository = reservationDetailRepository;
    }
}
