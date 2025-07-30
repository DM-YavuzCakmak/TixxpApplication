using Microsoft.AspNetCore.Mvc;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Business.Services.Abstract.RoleGroupRole;
using Tixxp.Business.Services.Abstract.RoleGroup;
using Tixxp.Entities.RoleGroup;
using Tixxp.Entities.Role;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.WebApp.Models.RoleGroupRole;
using Tixxp.Core.Utilities.Results.Concrete;

namespace Tixxp.WebApp.Controllers
{
    public class RoleGroupRoleController : Controller
    {
        private readonly IRoleGroupRoleService _roleGroupRoleService;
        private readonly IRoleGroupService _roleGroupService;
        private readonly IRoleService _roleService;

        public RoleGroupRoleController(
            IRoleGroupRoleService roleGroupRoleService,
            IRoleGroupService roleGroupService,
            IRoleService roleService)
        {
            _roleGroupRoleService = roleGroupRoleService;
            _roleGroupService = roleGroupService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            // 1. RoleGroupRole listesi
            var roleGroupRoles = _roleGroupRoleService
                .GetList(x => !x.IsDeleted).Data ?? new List<RoleGroupRoleEntity>();

            // 2. Role'leri çek
            var roleIds = roleGroupRoles
                .Where(x => x.RoleId != null)
                .Select(x => x.RoleId)
                .Distinct()
                .ToList();

            var roles = _roleService
                .GetList(x => roleIds.Contains(x.Id)).Data ?? new List<RoleEntity>();

            // 3. RoleGroup'ları çek
            var roleGroupIds = roleGroupRoles
                .Where(x => x.RoleGroupId != null)
                .Select(x => x.RoleGroupId)
                .Distinct()
                .ToList();

            var roleGroups = _roleGroupService
                .GetList(x => roleGroupIds.Contains(x.Id)).Data ?? new List<RoleGroupEntity>();

            // 4. Eşlemeleri yap
            foreach (var item in roleGroupRoles)
            {
                item.Role = roles.FirstOrDefault(r => r.Id == item.RoleId);
                item.RoleGroup = roleGroups.FirstOrDefault(g => g.Id == item.RoleGroupId);
            }

            return View(roleGroupRoles);
        }

        [HttpGet]
        public IActionResult GetUnassignedRoleGroupsAndAllRoles()
        {
            var assignedRoleGroupIds = _roleGroupRoleService
                .GetList(x => !x.IsDeleted)
                .Data
                .Select(x => x.RoleGroupId)
                .Distinct()
                .ToList();

            var unassignedGroups = _roleGroupService
                .GetList(x => !x.IsDeleted && !assignedRoleGroupIds.Contains(x.Id))
                .Data
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList();

            var allRoles = _roleService
                .GetList(x => !x.IsDeleted)
                .Data
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList();

            return Json(new
            {
                success = true,
                roleGroups = unassignedGroups,
                roles = allRoles
            });
        }

        [HttpPost]
        public IActionResult AssignRoles([FromBody] AssignRoleGroupRolesRequest request)
        {
            if (request == null || request.RoleGroupId <= 0 || request.RoleIds == null || !request.RoleIds.Any())
            {
                return Json(new { success = false, message = "Geçersiz istek verisi." });
            }

            var newEntities = request.RoleIds.Select(roleId => new RoleGroupRoleEntity
            {
                RoleGroupId = request.RoleGroupId,
                RoleId = roleId,
                CreatedBy = 6, // TODO: Giriş yapan kullanıcıdan alınmalı
                Created_Date = DateTime.Now,
                IsDeleted = false
            }).ToList();

            var result = _roleGroupRoleService.AddRange(newEntities);

            return Json(new
            {
                success = result.Success,
                message = result.Success ? "Roller başarıyla atandı." : result.Message
            });

        }
    }
}
