using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Agency;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Agency;

public class AgencyRepository : EfEntityRepositoryBase<AgencyEntity, CommonDbContext>, IAgencyRepository
{
    public AgencyRepository(CommonDbContext context) : base(context)
    {
    }
}
