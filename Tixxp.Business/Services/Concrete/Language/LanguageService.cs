using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Language;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.Language;

public class LanguageService : BaseService<LanguageEntity>, ILanguageService
{
    private readonly ILanguageRepository _languageRepository;


    public LanguageService(ILanguageRepository languageRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(languageRepository, logService, httpContextAccessor)
    {
        _languageRepository = languageRepository;
    }
}
