using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CampaignAction;
using Tixxp.Entities.CampaignCondition;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignCondition;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CampaignCondition;

public class CampaignConditionRepository : EfEntityRepositoryBase<CampaignConditionEntity, TixappContext>, ICampaignConditionRepository
{
    public CampaignConditionRepository(TixappContext context) : base(context)
    {
    }
}
