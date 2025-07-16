using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Language;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Language;

public class LanguageRepository : EfEntityRepositoryBase<LanguageEntity, TixappContext>, ILanguageRepository
{
    public LanguageRepository(TixappContext context) : base(context)
    {
    }
}
