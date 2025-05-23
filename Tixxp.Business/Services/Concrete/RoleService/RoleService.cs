using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
namespace Tixxp.Business.Services.Concrete.RoleService;
public class RoleService : BaseService<RoleEntity>, IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
        : base(roleRepository)
    {
        _roleRepository = roleRepository;
    }
}
