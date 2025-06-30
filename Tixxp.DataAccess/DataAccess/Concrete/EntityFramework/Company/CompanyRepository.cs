using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Company;
using Tixxp.Infrastructure.DataAccess.Abstract.Company;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Museum;

public class CompanyRepository : EfEntityRepositoryBase<CompanyEntity, CommonDbContext>, ICompanyRepository
{
    public CompanyRepository(CommonDbContext context) : base(context)
    {
    }
}
