using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Entities.Agency;

namespace Tixxp.WebApp.Controllers
{
    public class AgencyController : Controller
    {
        private readonly IAgencyService _agencyService;

        public AgencyController(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }

        public IActionResult Index()
        {
            var agencyList = _agencyService.GetList(x => x.IsDeleted == false);
            return View(agencyList.Data);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _agencyService.GetById(id);
            if (result.Success && result.Data != null)
            {
                return Json(new
                {
                    success = true,
                    id = result.Data.Id,
                    name = result.Data.Name
                });
            }

            return Json(new { success = false, message = "Acente bulunamadı." });
        }

        [HttpPost]
        public IActionResult Create([FromBody] AgencyEntity model)
        {
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim == null)
                return RedirectToAction("Login", "Auth");

            long personnelId = Convert.ToInt64(personnelIdClaim.Value);

            model.CreatedBy = personnelId;
            model.Created_Date = DateTime.Now;
            var result = _agencyService.Add(model);
            if (result.Success)
            {
                return Json(new { success = true, message = "Acente başarıyla eklendi." });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] AgencyEntity model)
        {
            var existing = _agencyService.GetById(model.Id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Acente bulunamadı." });

            var entity = existing.Data;
            entity.Name = model.Name;

            var result = _agencyService.Update(entity);
            if (result.Success)
            {
                return Json(new { success = true, message = "Acente başarıyla güncellendi." });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _agencyService.GetById(id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Acente bulunamadı." });

            var entity = existing.Data;
            entity.IsDeleted = true; // Soft delete

            var result = _agencyService.Update(entity);
            if (result.Success)
            {
                return Json(new { success = true, message = "Acente başarıyla silindi." });
            }

            return Json(new { success = false, message = result.Message });
        }
    }
}
