using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Campaign;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Campaign;

public interface ICampaignRepository : IEntityRepository<CampaignEntity>
{
}
