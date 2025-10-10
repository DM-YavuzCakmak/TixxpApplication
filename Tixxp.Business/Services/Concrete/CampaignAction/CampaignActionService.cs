using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.CampaignAction;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.CampaignAction;

public class CampaignActionService : BaseService<CampaignActionEntity>, ICampaignActionService
{
    private readonly ICampaignActionRepository _campaignActionRepository;
    public CampaignActionService(ICampaignActionRepository campaignActionRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(campaignActionRepository, logService, httpContextAccessor)
    {
        _campaignActionRepository = campaignActionRepository;
    }
}
