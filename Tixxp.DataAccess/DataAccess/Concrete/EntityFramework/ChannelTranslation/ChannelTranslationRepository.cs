using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Channel;
using Tixxp.Entities.ChannelTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.ChannelTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ChannelTranslation;

public class ChannelTranslationRepository : EfEntityRepositoryBase<ChannelTranslationEntity, TixappContext>, IChannelTranslationRepository
{
    public ChannelTranslationRepository(TixappContext context) : base(context)
    {
    }
}