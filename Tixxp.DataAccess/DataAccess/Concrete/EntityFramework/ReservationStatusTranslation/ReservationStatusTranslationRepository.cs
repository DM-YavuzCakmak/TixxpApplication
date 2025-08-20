using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Entities.ReservationStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationStatusTranslation;

public class ReservationStatusTranslationRepository : EfEntityRepositoryBase<ReservationStatusTranslationEntity, TixappContext>, IReservationStatusTranslationRepository
{
    public ReservationStatusTranslationRepository(TixappContext context) : base(context)
    {
    }
}
