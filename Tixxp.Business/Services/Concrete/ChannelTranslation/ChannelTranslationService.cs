using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Channel;
using Tixxp.Business.Services.Abstract.ChannelTranslation;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Channel;
using Tixxp.Entities.ChannelTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.ChannelTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.ChannelTranslation;

public class ChannelTranslationService : BaseService<ChannelTranslationEntity>, IChannelTranslationService
{
    private readonly IChannelTranslationRepository _channelTranslatioRepository;
    public ChannelTranslationService(IChannelTranslationRepository channelTranslatioRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(channelTranslatioRepository, logService, httpContextAccessor)
    {
        _channelTranslatioRepository = channelTranslatioRepository;
    }
}
