using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Museum;
using Tixxp.Infrastructure.DataAccess.Abstract.Museum;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Museum;

public class MuseumRepository : EfEntityRepositoryBase<MuseumEntity, CommonDbContext>, IMuseumRepository
{
    public MuseumRepository(CommonDbContext context) : base(context)
    {
    }
}
