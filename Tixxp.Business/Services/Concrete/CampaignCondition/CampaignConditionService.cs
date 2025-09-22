using Tixxp.Business.Services.Abstract.CampaignCondition;
using Tixxp.Business.Services.Abstract.CampaignConditionType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CampaignCondition;
using Tixxp.Entities.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignCondition;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;

namespace Tixxp.Business.Services.Concrete.CampaignCondition;

public class CampaignConditionService : BaseService<CampaignConditionEntity>, ICampaignConditionService
{
    private readonly ICampaignConditionRepository _campaignConditionRepository;
    public CampaignConditionService(ICampaignConditionRepository campaignConditionRepository)
        : base(campaignConditionRepository)
    {
        _campaignConditionRepository = campaignConditionRepository;
    }
}
