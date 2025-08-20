using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationSaleInvoiceInfo;

namespace Tixxp.Business.Services.Concrete.ReservationSaleInvoiceInfo;

public class ReservationSaleInvoiceInfoService : BaseService<ReservationSaleInvoiceInfoEntity>, IReservationSaleInvoiceInfoService
{
    private readonly IReservationSaleInvoiceInfoRepository _reservationSaleInvoiceInfoRepository;
    public ReservationSaleInvoiceInfoService(IReservationSaleInvoiceInfoRepository reservationSaleInvoiceInfoRepository)
        : base(reservationSaleInvoiceInfoRepository)
    {
        _reservationSaleInvoiceInfoRepository = reservationSaleInvoiceInfoRepository;
    }
}
