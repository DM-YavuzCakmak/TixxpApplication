using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.CityTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.CityTranslation;

public interface ICityTranslationRepository : IEntityRepository<CityTranslationEntity>
{
}
