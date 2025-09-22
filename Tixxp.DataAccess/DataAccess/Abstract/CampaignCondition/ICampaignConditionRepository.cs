using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CampaignAction;
using Tixxp.Entities.CampaignCondition;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CampaignCondition;

public interface ICampaignConditionRepository : IEntityRepository<CampaignConditionEntity>
{
}
