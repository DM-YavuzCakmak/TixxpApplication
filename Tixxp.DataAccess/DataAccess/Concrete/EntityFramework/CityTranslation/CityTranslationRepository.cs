using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.City;
using Tixxp.Entities.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.City;
using Tixxp.Infrastructure.DataAccess.Abstract.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CityTranslation;

public class CityTranslationRepository : EfEntityRepositoryBase<CityTranslationEntity, CommonDbContext>, ICityTranslationRepository
{
    public CityTranslationRepository(CommonDbContext context) : base(context)
    {
    }
}