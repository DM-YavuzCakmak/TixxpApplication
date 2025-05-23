using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Role;

public class RoleRepository : EfEntityRepositoryBase<RoleEntity, CommonDbContext>, IRoleRepository
{
    public RoleRepository(CommonDbContext context) : base(context)
    {
    }
}
