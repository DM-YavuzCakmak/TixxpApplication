using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;

public class PersonnelRoleRepository : EfEntityRepositoryBase<PersonnelRoleEntity, CommonDbContext>, IPersonnelRoleRepository
{
    public PersonnelRoleRepository(CommonDbContext context) : base(context)
    {
    }
}
