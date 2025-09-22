using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Campaign;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Campaign;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Campaign;

public class CampaignRepository : EfEntityRepositoryBase<CampaignEntity, TixappContext>, ICampaignRepository
{
    public CampaignRepository(TixappContext context) : base(context)
    {
    }
}
