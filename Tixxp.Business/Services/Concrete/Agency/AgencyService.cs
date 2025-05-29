using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Agency;
using Tixxp.Infrastructure.DataAccess.Abstract.Agency;
namespace Tixxp.Business.Services.Concrete.Agency;

public class AgencyService : BaseService<AgencyEntity>, IAgencyService
{
    private readonly IAgencyRepository _agencyRepository;


    public AgencyService(IAgencyRepository agencyRepository)
        : base(agencyRepository)
    {
        _agencyRepository = agencyRepository;
    }
}
