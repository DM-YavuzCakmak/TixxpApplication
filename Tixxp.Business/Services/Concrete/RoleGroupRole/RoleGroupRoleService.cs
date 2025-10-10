using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.RoleGroupRole;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.RoleGroupRole;

public class RoleGroupRoleService : BaseService<RoleGroupRoleEntity>, IRoleGroupRoleService
{
    private readonly IRoleGroupRoleRepository _roleGroupRoleRepository;

    public RoleGroupRoleService(IRoleGroupRoleRepository roleGroupRoleRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(roleGroupRoleRepository, logService, httpContextAccessor)
    {
        _roleGroupRoleRepository = roleGroupRoleRepository;
    }
}
