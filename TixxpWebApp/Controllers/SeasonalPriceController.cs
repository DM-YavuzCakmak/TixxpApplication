using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.SeasonalPrice;
using Tixxp.Entities.SeasonalPrice;
using System;
using Tixxp.Business.Services.Abstract.Company;

namespace Tixxp.WebApp.Controllers
{
    public class SeasonalPriceController : Controller
    {
        private readonly ISeasonalPriceService _seasonalPriceService;
        private readonly ICompanyService _companyService;

        public SeasonalPriceController(ISeasonalPriceService seasonalPriceService, ICompanyService companyService)
        {
            _seasonalPriceService = seasonalPriceService;
            _companyService = companyService;
        }

        public IActionResult Index()
        {
            var seasonalPriceEntities = _seasonalPriceService.GetList(x => !x.IsDeleted).Data;
            if (seasonalPriceEntities != null)
            {
                foreach (var seasonalPriceEntity in seasonalPriceEntities)
                {
                    seasonalPriceEntity.Company = _companyService.GetFirstOrDefault(m => m.Id == seasonalPriceEntity.MuseumId && !m.IsDeleted).Data;
                }
            }
            ViewBag.Museums = _companyService.GetList(x => !x.IsDeleted).Data;
            return View(seasonalPriceEntities);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _seasonalPriceService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                museumId = result.Data.MuseumId,
                seasonName = result.Data.SeasonName,
                startDate = result.Data.StartDate.ToString("yyyy-MM-dd"),
                endDate = result.Data.EndDate.ToString("yyyy-MM-dd")
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] SeasonalPriceEntity model)
        {
            model.CreatedBy = 6;
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            var result = _seasonalPriceService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] SeasonalPriceEntity model)
        {
            var existing = _seasonalPriceService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.MuseumId = model.MuseumId;
            existing.Data.SeasonName = model.SeasonName;
            existing.Data.StartDate = model.StartDate;
            existing.Data.EndDate = model.EndDate;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _seasonalPriceService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _seasonalPriceService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            result.Data.IsDeleted = true;
            var updateResult = _seasonalPriceService.Update(result.Data);
            return Json(new { success = updateResult.Success, message = updateResult.Message });
        }
    }
}
