using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CampaignAction;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;

public interface ICampaignActionRepository : IEntityRepository<CampaignActionEntity>
{
}
