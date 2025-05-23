using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;

namespace Tixxp.Business.Services.Concrete.PersonnelRoleService;

public class PersonnelRoleService : BaseService<PersonnelRoleEntity>, IPersonnelRoleService
{
    private readonly IPersonnelRoleRepository _personnelRoleRepository;

    public PersonnelRoleService(IPersonnelRoleRepository personnelRoleRepository)
        : base(personnelRoleRepository)
    {
        _personnelRoleRepository = personnelRoleRepository;
    }
}
