using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.RoleGroup;
using Tixxp.Business.Services.Abstract.Template;
using Tixxp.Entities.RoleGroup;

namespace Tixxp.WebApp.Controllers
{
    public class RoleGroupController : Controller
    {
        private readonly IRoleGroupService _roleGroupService;
        private readonly ITemplateService _templateService;

        public RoleGroupController(IRoleGroupService roleGroupService, ITemplateService templateService)
        {
            _roleGroupService = roleGroupService;
            _templateService = templateService;
        }

        public IActionResult Index()
        {
            ViewBag.RoleGroupTemplates = _templateService.GetTemplateList(); 
            ViewBag.AllRoles = _templateService.GetAllRoles();

            var result = _roleGroupService.GetList(x => !x.IsDeleted);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _roleGroupService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                name = result.Data.Name
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] RoleGroupEntity model)
        {
            model.CreatedBy = 6; // TODO: Giriş yapan kullanıcı ID'siyle değiştirilebilir
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _roleGroupService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] RoleGroupEntity model)
        {
            var existing = _roleGroupService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.Name = model.Name;
            existing.Data.UpdatedBy = 6; // TODO: Giriş yapan kullanıcı ID'siyle değiştirilebilir
            existing.Data.Updated_Date = DateTime.Now;

            var result = _roleGroupService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpGet]
        public IActionResult GetRoles(int templateId)
        {
            if (templateId <= 0)
                return BadRequest("Geçersiz template ID.");

            var roles = _templateService.GetRolesByTemplateId(templateId);
            var result = roles.Select(r => new
            {
                id = r.Id,
                name = r.Name
            });

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetAvailableRoleGroupName(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
                return BadRequest("Geçersiz isim.");

            var existingNames = _roleGroupService
                .GetList(x => !x.IsDeleted)
                .Data
                .Select(x => x.Name)
                .ToList();

            if (!existingNames.Contains(baseName))
                return Json(baseName);

            int counter = 1;
            string newName;
            do
            {
                newName = $"{baseName} {counter}";
                counter++;
            } while (existingNames.Contains(newName));

            return Json(newName);
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _roleGroupService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.IsDeleted = true;
            existing.Data.UpdatedBy = 6;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _roleGroupService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
