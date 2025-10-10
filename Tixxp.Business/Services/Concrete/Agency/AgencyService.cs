using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Agency;
using Tixxp.Infrastructure.DataAccess.Abstract.Agency;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;
namespace Tixxp.Business.Services.Concrete.Agency;

public class AgencyService : BaseService<AgencyEntity>, IAgencyService
{
    private readonly IAgencyRepository _agencyRepository;

    public AgencyService(IAgencyRepository agencyRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(agencyRepository, logService, httpContextAccessor)
    {
        _agencyRepository = agencyRepository;
    }
}
