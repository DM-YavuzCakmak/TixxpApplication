using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroup;

public class RoleGroupRepository : EfEntityRepositoryBase<RoleGroupEntity, CommonDbContext>, IRoleGroupRepository
{
    public RoleGroupRepository(CommonDbContext context) : base(context)
    {
    }
}
