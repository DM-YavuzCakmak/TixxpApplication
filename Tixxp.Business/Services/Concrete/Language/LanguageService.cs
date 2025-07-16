using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Language;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;

namespace Tixxp.Business.Services.Concrete.Language;

public class LanguageService : BaseService<LanguageEntity>, ILanguageService
{
    private readonly ILanguageRepository _languageRepository;


    public LanguageService(ILanguageRepository languageRepository)
        : base(languageRepository)
    {
        _languageRepository = languageRepository;
    }
}
