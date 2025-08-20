using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationSaleInvoiceInfo;

public class ReservationSaleInvoiceInfoRepository : EfEntityRepositoryBase<ReservationSaleInvoiceInfoEntity, TixappContext>, IReservationSaleInvoiceInfoRepository
{
    public ReservationSaleInvoiceInfoRepository(TixappContext context) : base(context)
    {
    }
}