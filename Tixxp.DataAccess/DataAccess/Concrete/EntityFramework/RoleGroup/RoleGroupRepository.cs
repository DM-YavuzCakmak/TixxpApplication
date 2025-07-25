using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroup;

public class RoleGroupRepository : EfEntityRepositoryBase<RoleGroupEntity, TixappContext>, IRoleGroupRepository
{
    public RoleGroupRepository(TixappContext context) : base(context)
    {
    }
}
