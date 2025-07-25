using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Context;
namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;

public class PersonnelRoleGroupRepository : EfEntityRepositoryBase<PersonnelRoleGroupEntity, TixappContext>, IPersonnelRoleGroupRepository
{
    public PersonnelRoleGroupRepository(TixappContext context) : base(context)
    {
    }
}
