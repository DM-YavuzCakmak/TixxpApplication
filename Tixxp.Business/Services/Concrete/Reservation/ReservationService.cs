using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;

namespace Tixxp.Business.Services.Concrete.Reservation;

public class ReservationService : BaseService<ReservationEntity>, IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    public ReservationService(IReservationRepository reservationRepository)
        : base(reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }
}
