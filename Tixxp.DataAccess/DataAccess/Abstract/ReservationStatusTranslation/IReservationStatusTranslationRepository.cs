using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ReservationStatusTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatusTranslation;

public interface IReservationStatusTranslationRepository : IEntityRepository<ReservationStatusTranslationEntity>
{
}
