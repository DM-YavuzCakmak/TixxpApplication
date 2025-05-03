using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Role;

public class RoleRepository : EfEntityRepositoryBase<RoleEntity, TixappContext>, IRoleRepository
{
    public RoleRepository(TixappContext context) : base(context)
    {
    }
}
