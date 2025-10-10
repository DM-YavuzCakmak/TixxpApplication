using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Guide;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Guide;
using Tixxp.Infrastructure.DataAccess.Abstract.Guide;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.Guide;

public class GuideService : BaseService<GuideEntity>, IGuideService
{
    private readonly IGuideRepository _guideRepository;

    public GuideService(IGuideRepository guideRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(guideRepository, logService, httpContextAccessor)
    {
        _guideRepository = guideRepository;
    }
}
