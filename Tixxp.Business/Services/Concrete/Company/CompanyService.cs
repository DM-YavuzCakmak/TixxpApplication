using Tixxp.Business.Services.Abstract.Company;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Company;
using Tixxp.Infrastructure.DataAccess.Abstract.Company;

namespace Tixxp.Business.Services.Concrete.Company;

public class CompanyService : BaseService<CompanyEntity>, ICompanyService
{
    private readonly ICompanyRepository _companyRepository;


    public CompanyService(ICompanyRepository companyRepository)
        : base(companyRepository)
    {
        _companyRepository = companyRepository;
    }
}
