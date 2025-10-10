using Microsoft.AspNetCore.Http;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.RoleGroup;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;

namespace Tixxp.Business.Services.Concrete.RoleGroup;

public class RoleGroupService : BaseService<RoleGroupEntity>, IRoleGroupService
{
    private readonly IRoleGroupRepository _roleGroupRepository;

    public RoleGroupService(IRoleGroupRepository roleGroupRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(roleGroupRepository, logService, httpContextAccessor)
    {
        _roleGroupRepository = roleGroupRepository;
    }
}
