using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Guide;
using Tixxp.Entities.Guide;

namespace Tixxp.WebApp.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;

        public GuideController(IGuideService guideService)
        {
            _guideService = guideService;
        }

        public IActionResult Index()
        {
            var guideList = _guideService.GetList(x => x.IsActive == true);
            return View(guideList.Data);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _guideService.GetById(id);
            if (result.Success && result.Data != null)
            {
                return Json(new
                {
                    success = true,
                    id = result.Data.Id,
                    name = result.Data.Name,
                    nationalIdNumber = result.Data.NationalIdNumber,
                    gsmNumber = result.Data.GsmNumber,
                    isGsmConfirmed = result.Data.IsGsmConfirmed,
                    email = result.Data.Email,
                    isEmailConfirmed = result.Data.IsEmailConfirmed,
                    licenseNumber = result.Data.LicenseNumber
                });
            }

            return Json(new { success = false, message = "Rehber bulunamadı." });
        }

        [HttpPost]
        public IActionResult Create([FromBody] GuideEntity model)
        {
            model.IsActive = true;
            var result = _guideService.Add(model);
            if (result.Success)
            {
                return Json(new { success = true, message = "Rehber başarıyla eklendi." });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] GuideEntity model)
        {
            var existing = _guideService.GetById(model.Id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Rehber bulunamadı." });

            var entity = existing.Data;

            entity.Name = model.Name;
            entity.NationalIdNumber = model.NationalIdNumber;
            entity.GsmNumber = model.GsmNumber;
            entity.IsGsmConfirmed = model.IsGsmConfirmed;
            entity.Email = model.Email;
            entity.IsEmailConfirmed = model.IsEmailConfirmed;
            entity.LicenseNumber = model.LicenseNumber;

            var result = _guideService.Update(entity);
            if (result.Success)
            {
                return Json(new { success = true, message = "Rehber başarıyla güncellendi." });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _guideService.GetById(id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Rehber bulunamadı." });

            var entity = existing.Data;
            entity.IsActive = false;
            var result = _guideService.Update(entity); // Soft delete

            if (result.Success)
            {
                return Json(new { success = true, message = "Rehber başarıyla silindi." });
            }

            return Json(new { success = false, message = result.Message });
        }
    }
}
