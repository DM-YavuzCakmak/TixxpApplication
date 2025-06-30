using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Company;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Company;

public interface ICompanyRepository : IEntityRepository<CompanyEntity>
{
}
