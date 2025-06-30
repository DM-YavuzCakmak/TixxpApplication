using Microsoft.AspNetCore.Mvc;
using Tixxp.Entities.Role;
using System;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;

namespace Tixxp.WebApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IPersonnelRoleService _personnelRoleService;

        public RoleController(IRoleService roleService, IPersonnelRoleService personnelRoleService)
        {
            _roleService = roleService;
            _personnelRoleService = personnelRoleService;
        }

        public IActionResult Index()
        {
            var roles = _roleService.GetList(x => !x.IsDeleted).Data;
            return View(roles);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _roleService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                name = result.Data.Name
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] RoleEntity model)
        {
            model.CreatedBy = 6;
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _roleService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] RoleEntity model)
        {
            var existing = _roleService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.Name = model.Name;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _roleService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _roleService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            var personnelRoles = _personnelRoleService.GetList(x => x.RoleId == id && !x.IsDeleted);
            if (personnelRoles.Data.Count > 0)
            {
                return Json(new { success = false, message = "Bu role bağlı personel kayıtları bulunmaktadır. Silme işlemi yapılamaz." });
            }

            existing.Data.IsDeleted = true;
            var result = _roleService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
