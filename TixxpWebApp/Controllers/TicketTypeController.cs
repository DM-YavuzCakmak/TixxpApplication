﻿using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Entities.TicketType;

namespace Tixxp.WebApp.Controllers
{
    public class TicketTypeController : Controller
    {
        private readonly ITicketTypeService _ticketTypeService;

        public TicketTypeController(ITicketTypeService ticketTypeService)
        {
            _ticketTypeService = ticketTypeService;
        }

        public IActionResult Index()
        {
            var result = _ticketTypeService.GetList(x => !x.IsDeleted);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _ticketTypeService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (result.Success)
            {
                return Json(new
                {
                    id = result.Data.Id,
                    name = result.Data.Name
                });
            }
            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Create([FromBody] TicketTypeEntity model)
        {
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            var result = _ticketTypeService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] TicketTypeEntity model)
        {
            var existing = _ticketTypeService.GetFirstOrDefault(x => x.Id == model.Id);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.Name = model.Name;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _ticketTypeService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _ticketTypeService.GetFirstOrDefault(x => x.Id == id);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.IsDeleted = true;
            var result = _ticketTypeService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
