using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroupRole;

public class RoleGroupRoleRepository : EfEntityRepositoryBase<RoleGroupRoleEntity, CommonDbContext>, IRoleGroupRoleRepository
{
    public RoleGroupRoleRepository(CommonDbContext context) : base(context)
    {
    }
}
