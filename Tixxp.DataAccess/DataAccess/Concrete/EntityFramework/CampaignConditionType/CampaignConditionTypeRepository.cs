using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CampaignConditionType;

public class CampaignConditionTypeRepository : EfEntityRepositoryBase<CampaignConditionTypeEntity, TixappContext>, ICampaignConditionTypeRepository
{
    public CampaignConditionTypeRepository(TixappContext context) : base(context)
    {
    }
}
