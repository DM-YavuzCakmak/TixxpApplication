using Tixxp.Business.Services.Abstract.AgencyContract;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.AgencyContract;

namespace Tixxp.Business.Services.Concrete.AgencyContract;

public class AgencyContractService : BaseService<AgencyContractEntity>, IAgencyContractService
{
    private readonly IAgencyContractRepository _agencyContractRepository;


    public AgencyContractService(IAgencyContractRepository agencyContractRepository)
        : base(agencyContractRepository)
    {
        _agencyContractRepository = agencyContractRepository;
    }
}