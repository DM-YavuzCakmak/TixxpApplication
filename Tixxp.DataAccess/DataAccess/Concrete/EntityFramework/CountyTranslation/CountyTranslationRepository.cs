using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Abstract.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CountyTranslation;

public class CountyTranslationRepository : EfEntityRepositoryBase<CountyTranslationEntity, CommonDbContext>, ICountyTranslationRepository
{
    public CountyTranslationRepository(CommonDbContext context) : base(context)
    {
    }
}
