using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.Reservation;

public class ReservationService : BaseService<ReservationEntity>, IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    public ReservationService(IReservationRepository reservationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(reservationRepository, logService, httpContextAccessor)
    {
        _reservationRepository = reservationRepository;
    }
}
