using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CampaignAction;
public class CampaignActionRepository : EfEntityRepositoryBase<CampaignActionEntity, TixappContext>, ICampaignActionRepository
{
    public CampaignActionRepository(TixappContext context) : base(context)
    {
    }
}
