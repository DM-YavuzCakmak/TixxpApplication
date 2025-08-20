using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.ReservationStatus;
using Tixxp.Business.Services.Abstract.ReservationStatusTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Entities.ReservationStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatusTranslation;

namespace Tixxp.Business.Services.Concrete.ReservationStatusTranslation;

public class ReservationStatusTranslationService : BaseService<ReservationStatusTranslationEntity>, IReservationStatusTranslationService
{
    private readonly IReservationStatusTranslationRepository _reservationStatusTranslationRepository;
    public ReservationStatusTranslationService(IReservationStatusTranslationRepository reservationStatusTranslationRepository)
        : base(reservationStatusTranslationRepository)
    {
        _reservationStatusTranslationRepository = reservationStatusTranslationRepository;
    }
}
