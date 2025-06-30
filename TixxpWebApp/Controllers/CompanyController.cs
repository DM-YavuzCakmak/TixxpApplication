using Microsoft.AspNetCore.Mvc;
using System;
using Tixxp.Business.Services.Abstract.Company;
using Tixxp.Entities.Company;

namespace Tixxp.WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _museumService;

        public CompanyController(ICompanyService museumService)
        {
            _museumService = museumService;
        }

        public IActionResult Index()
        {
            var museums = _museumService.GetList(x => !x.IsDeleted).Data;
            return View(museums);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _museumService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                name = result.Data.Name,
                identifier = result.Data.Identifier
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] CompanyEntity model)
        {
            model.CreatedBy = 6;
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            var result = _museumService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] CompanyEntity model)
        {
            var existing = _museumService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.Name = model.Name;
            existing.Data.Identifier = model.Identifier;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _museumService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _museumService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.IsDeleted = true;
            var result = _museumService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
