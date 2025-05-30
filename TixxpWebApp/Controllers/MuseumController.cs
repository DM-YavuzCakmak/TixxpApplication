using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Museum;
using Tixxp.Entities.Museum;
using System;

namespace Tixxp.WebApp.Controllers
{
    public class MuseumController : Controller
    {
        private readonly IMuseumService _museumService;

        public MuseumController(IMuseumService museumService)
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
        public IActionResult Create([FromBody] MuseumEntity model)
        {
            model.CreatedBy = 6;
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            var result = _museumService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] MuseumEntity model)
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
