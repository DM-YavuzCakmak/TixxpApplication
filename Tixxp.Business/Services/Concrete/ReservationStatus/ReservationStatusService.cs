using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.ReservationStatus;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ReservationStatus;

public class ReservationStatusService : BaseService<ReservationStatusEntity>, IReservationStatusService
{
    private readonly IReservationStatusRepository _reservationStatusRepository;
    public ReservationStatusService(IReservationStatusRepository reservationStatusRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(reservationStatusRepository, logService, httpContextAccessor)
    {
        _reservationStatusRepository = reservationStatusRepository;
    }
}
