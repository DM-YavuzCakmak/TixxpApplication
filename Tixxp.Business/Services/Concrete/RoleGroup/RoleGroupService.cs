using Tixxp.Business.Services.Abstract.RoleGroup;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;

namespace Tixxp.Business.Services.Concrete.RoleGroup;

public class RoleGroupService : BaseService<RoleGroupEntity>, IRoleGroupService
{
    private readonly IRoleGroupRepository _roleGroupRepository;

    public RoleGroupService(IRoleGroupRepository roleGroupRepository)
        : base(roleGroupRepository)
    {
        _roleGroupRepository = roleGroupRepository;
    }
}
