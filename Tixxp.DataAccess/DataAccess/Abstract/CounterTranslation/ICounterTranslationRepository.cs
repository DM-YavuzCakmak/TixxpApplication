using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Counter;
using Tixxp.Entities.CounterTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CounterTranslation;

public interface ICounterTranslationRepository : IEntityRepository<CounterTranslationEntity>
{
}
