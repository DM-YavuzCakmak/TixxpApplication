using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CountryTranslation;

public class CountryTranslationRepository : EfEntityRepositoryBase<CountryTranslationEntity, CommonDbContext>, ICountryTranslationRepository
{
    public CountryTranslationRepository(CommonDbContext context) : base(context)
    {
    }
}

