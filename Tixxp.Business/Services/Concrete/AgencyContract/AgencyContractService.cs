using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.AgencyContract;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.AgencyContract;

public class AgencyContractService : BaseService<AgencyContractEntity>, IAgencyContractService
{
    private readonly IAgencyContractRepository _agencyContractRepository;


    public AgencyContractService(IAgencyContractRepository agencyContractRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(agencyContractRepository, logService, httpContextAccessor)
    {
        _agencyContractRepository = agencyContractRepository;
    }
}