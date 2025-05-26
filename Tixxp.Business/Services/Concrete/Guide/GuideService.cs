using Tixxp.Business.Services.Abstract.Guide;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Guide;
using Tixxp.Infrastructure.DataAccess.Abstract.Guide;

namespace Tixxp.Business.Services.Concrete.Guide;

public class GuideService : BaseService<GuideEntity>, IGuideService
{
    private readonly IGuideRepository _guideRepository;

    public GuideService(IGuideRepository guideRepository)
        : base(guideRepository)
    {
        _guideRepository = guideRepository;
    }
}
