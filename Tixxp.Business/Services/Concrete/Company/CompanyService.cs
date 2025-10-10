using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Company;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Company;
using Tixxp.Infrastructure.DataAccess.Abstract.Company;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;

namespace Tixxp.Business.Services.Concrete.Company;

public class CompanyService : BaseService<CompanyEntity>, ICompanyService
{
    private readonly ICompanyRepository _companyRepository;


    public CompanyService(ICompanyRepository companyRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(companyRepository, logService, httpContextAccessor)
    {
        _companyRepository = companyRepository;
    }
}
