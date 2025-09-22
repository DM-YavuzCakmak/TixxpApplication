using Tixxp.Business.Services.Abstract.CampaignAction;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;

namespace Tixxp.Business.Services.Concrete.CampaignAction;

public class CampaignActionService : BaseService<CampaignActionEntity>, ICampaignActionService
{
    private readonly ICampaignActionRepository _campaignActionRepository;
    public CampaignActionService(ICampaignActionRepository campaignActionRepository)
        : base(campaignActionRepository)
    {
        _campaignActionRepository = campaignActionRepository;
    }
}
