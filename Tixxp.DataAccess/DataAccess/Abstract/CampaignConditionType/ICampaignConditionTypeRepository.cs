using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Campaign;
using Tixxp.Entities.CampaignConditionType;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;

public interface ICampaignConditionTypeRepository : IEntityRepository<CampaignConditionTypeEntity>
{
}
