using Tixxp.Business.Services.Abstract.CampaignConditionType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;

namespace Tixxp.Business.Services.Concrete.CampaignConditionType;

internal class CampaignConditionTypeService : BaseService<CampaignConditionTypeEntity>, ICampaignConditionTypeService
{
    private readonly ICampaignConditionTypeRepository _campaignConditionTypeRepository;
    public CampaignConditionTypeService(ICampaignConditionTypeRepository campaignConditionTypeRepository)
        : base(campaignConditionTypeRepository)
    {
        _campaignConditionTypeRepository = campaignConditionTypeRepository;
    }
}
