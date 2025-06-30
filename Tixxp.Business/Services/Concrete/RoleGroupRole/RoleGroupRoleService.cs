using Tixxp.Business.Services.Abstract.RoleGroupRole;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroupRole;

namespace Tixxp.Business.Services.Concrete.RoleGroupRole;

public class RoleGroupRoleService : BaseService<RoleGroupRoleEntity>, IRoleGroupRoleService
{
    private readonly IRoleGroupRoleRepository _roleGroupRoleRepository;

    public RoleGroupRoleService(IRoleGroupRoleRepository roleGroupRoleRepository)
        : base(roleGroupRoleRepository)
    {
        _roleGroupRoleRepository = roleGroupRoleRepository;
    }
}
