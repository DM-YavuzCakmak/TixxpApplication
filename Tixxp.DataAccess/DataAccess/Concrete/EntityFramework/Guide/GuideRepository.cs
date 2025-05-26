using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Guide;
using Tixxp.Infrastructure.DataAccess.Abstract.Guide;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Guide;

public class GuideRepository : EfEntityRepositoryBase<GuideEntity, TixappContext>, IGuideRepository
{
    public GuideRepository(TixappContext context) : base(context)
    {
    }
}
