using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;
namespace Tixxp.Business.Services.Concrete.RoleService;
public class RoleService : BaseService<RoleEntity>, IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(roleRepository, logService, httpContextAccessor)
    {
        _roleRepository = roleRepository;
    }
}
