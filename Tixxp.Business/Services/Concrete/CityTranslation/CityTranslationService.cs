using Tixxp.Business.Services.Abstract.CityTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.CityTranslation;

namespace Tixxp.Business.Services.Concrete.CityTranslation;

public class CityTranslationService : BaseService<CityTranslationEntity>, ICityTranslationService
{
    private readonly ICityTranslationRepository _cityTranslationRepository;
    public CityTranslationService(ICityTranslationRepository cityTranslationRepository)
        : base(cityTranslationRepository)
    {
        _cityTranslationRepository = cityTranslationRepository;
    }
}

