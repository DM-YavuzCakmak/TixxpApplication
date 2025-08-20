using Tixxp.Business.Services.Abstract.Channel;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Channel;
using Tixxp.Entities.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;

namespace Tixxp.Business.Services.Concrete.Channel;

public class ChannelService : BaseService<ChannelEntity>, IChannelService
{
    private readonly IChannelRepository _channelRepository;
    public ChannelService(IChannelRepository channelRepository)
        : base(channelRepository)
    {
        _channelRepository = channelRepository;
    }
}
