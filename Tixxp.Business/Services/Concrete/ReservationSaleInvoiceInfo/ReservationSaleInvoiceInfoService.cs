using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.ReservationSaleInvoiceInfo;

public class ReservationSaleInvoiceInfoService : BaseService<ReservationSaleInvoiceInfoEntity>, IReservationSaleInvoiceInfoService
{
    private readonly IReservationSaleInvoiceInfoRepository _reservationSaleInvoiceInfoRepository;
    public ReservationSaleInvoiceInfoService(IReservationSaleInvoiceInfoRepository reservationSaleInvoiceInfoRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(reservationSaleInvoiceInfoRepository, logService, httpContextAccessor)
    {
        _reservationSaleInvoiceInfoRepository = reservationSaleInvoiceInfoRepository;
    }
}
