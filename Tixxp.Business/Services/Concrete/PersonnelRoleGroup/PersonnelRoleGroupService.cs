using Tixxp.Business.Services.Abstract.PersonnelRoleGroup;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRoleGroup;

namespace Tixxp.Business.Services.Concrete.PersonnelRoleGroup;

public class PersonnelRoleGroupService : BaseService<PersonnelRoleGroupEntity>, IPersonnelRoleGroupService
{
    private readonly IPersonnelRoleGroupRepository _personnelRoleGroupRepository;

    public PersonnelRoleGroupService(IPersonnelRoleGroupRepository personnelRoleGroupRepository)
        : base(personnelRoleGroupRepository)
    {
        _personnelRoleGroupRepository = personnelRoleGroupRepository;
    }
}
