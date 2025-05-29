using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Entities.PriceCategory;

namespace Tixxp.WebApp.Controllers
{
    public class PriceCategoryController : Controller
    {
        private readonly IPriceCategoryService _priceCategoryService;

        public PriceCategoryController(IPriceCategoryService priceCategoryService)
        {
            _priceCategoryService = priceCategoryService;
        }

        public IActionResult Index()
        {
            var result = _priceCategoryService.GetList(x => x.IsDeleted == false);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _priceCategoryService.GetById(id);
            if (result.Success && result.Data != null)
            {
                return Json(new { success = true, id = result.Data.Id, name = result.Data.Name });
            }
            return Json(new { success = false, message = "Kategori bulunamadı." });
        }

        [HttpPost]
        public IActionResult Create([FromBody] PriceCategoryEntity model)
        {
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim == null)
                return RedirectToAction("Login", "Auth");

            long personnelId = Convert.ToInt64(personnelIdClaim.Value);

            model.CreatedBy = personnelId;
            model.Created_Date = DateTime.Now;
            var result = _priceCategoryService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] PriceCategoryEntity model)
        {
            var existing = _priceCategoryService.GetById(model.Id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Kategori bulunamadı." });

            existing.Data.Name = model.Name;
            var result = _priceCategoryService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _priceCategoryService.GetById(id);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = "Kategori bulunamadı." });

            existing.Data.IsDeleted = true;
            var result = _priceCategoryService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
