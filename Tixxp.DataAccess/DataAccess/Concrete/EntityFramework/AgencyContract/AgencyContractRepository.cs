using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Agency;
using Tixxp.Entities.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.AgencyContract;

public class AgencyContractRepository : EfEntityRepositoryBase<AgencyContractEntity, TixappContext>, IAgencyContractRepository
{
    public AgencyContractRepository(TixappContext context) : base(context)
    {
    }
}
