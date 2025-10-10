using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Channel;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;

namespace Tixxp.Business.Services.Concrete.Channel;

public class ChannelService : BaseService<ChannelEntity>, IChannelService
{
    private readonly IChannelRepository _channelRepository;
    public ChannelService(IChannelRepository channelRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(channelRepository, logService, httpContextAccessor)
    {
        _channelRepository = channelRepository;
    }
}
