using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.CampaignConditionType;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.CampaignConditionType;

internal class CampaignConditionTypeService : BaseService<CampaignConditionTypeEntity>, ICampaignConditionTypeService
{
    private readonly ICampaignConditionTypeRepository _campaignConditionTypeRepository;
    public CampaignConditionTypeService(ICampaignConditionTypeRepository campaignConditionTypeRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(campaignConditionTypeRepository, logService, httpContextAccessor)
    {
        _campaignConditionTypeRepository = campaignConditionTypeRepository;
    }
}
