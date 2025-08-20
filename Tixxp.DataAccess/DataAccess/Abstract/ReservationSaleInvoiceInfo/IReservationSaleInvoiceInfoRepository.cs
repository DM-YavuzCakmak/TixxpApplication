using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationSaleInvoiceInfo;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ReservationSaleInvoiceInfo;

public interface IReservationSaleInvoiceInfoRepository : IEntityRepository<ReservationSaleInvoiceInfoEntity>
{
}
