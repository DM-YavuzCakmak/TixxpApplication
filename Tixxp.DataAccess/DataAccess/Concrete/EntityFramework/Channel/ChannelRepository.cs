using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Channel;

public class ChannelRepository : EfEntityRepositoryBase<ChannelEntity, TixappContext>, IChannelRepository
{
    public ChannelRepository(TixappContext context) : base(context)
    {
    }
}