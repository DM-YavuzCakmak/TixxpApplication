using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.ChannelTranslation;
using Tixxp.Entities.CounterTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.ChannelTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CounterTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CounterTranslation;

public class CounterTranslationRepository : EfEntityRepositoryBase<CounterTranslationEntity, TixappContext>, ICounterTranslationRepository
{
    public CounterTranslationRepository(TixappContext context) : base(context)
    {
    }
}