using Microsoft.AspNetCore.Mvc;
using Tixxp.WebApp.Models;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Entities.Events;
using Tixxp.WebApp.Models.Event;

namespace Tixxp.WebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public IActionResult Index()
        {
            IDataResult<List<EventEntity>> result = _eventService.GetAll();
            return View(result.Success ? result.Data : new List<EventEntity>());
        }

        [HttpGet]
        public JsonResult GetById(long id)
        {
            var result = _eventService.GetById(id);
            if (result.Success)
                return Json(result.Data);

            return Json(null);
        }

        [HttpPost]
        public JsonResult Update([FromBody] EventDto dto)
        {
            if (dto == null)
                return Json(new { success = false, message = "Geçersiz veri." });

            var existingResult = _eventService.GetById(dto.Id);
            if (!existingResult.Success || existingResult.Data == null)
                return Json(new { success = false, message = "Etkinlik bulunamadı." });

            var entity = existingResult.Data;
            entity.Name = dto.Name;
            entity.StartTime = TimeSpan.TryParse(dto.StartTime, out var st) ? st : null;
            entity.EndTime = TimeSpan.TryParse(dto.EndTime, out var et) ? et : null;
            entity.DurationInMinutes = dto.DurationInMinutes;
            entity.IsAvailableOnB2C = dto.IsAvailableOnB2C;
            entity.IsAvailableOnB2B = dto.IsAvailableOnB2B;
            entity.Updated_Date = DateTime.Now;
            entity.UpdatedBy = 6; // Login olan kullanıcıdan alınmalı

            var result = _eventService.Update(entity);
            return result.Success
                ? Json(new { success = true, message = "Etkinlik başarıyla güncellendi." })
                : Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        public JsonResult Create([FromBody] EventDto dto)
        {
            if (dto != null)
            {
                var entity = new EventEntity
                {
                    Name = dto.Name,
                    StartTime = TimeSpan.TryParse(dto.StartTime, out var st) ? st : null,
                    EndTime = TimeSpan.TryParse(dto.EndTime, out var et) ? et : null,
                    DurationInMinutes = dto.DurationInMinutes,
                    IsAvailableOnB2C = dto.IsAvailableOnB2C,
                    IsAvailableOnB2B = dto.IsAvailableOnB2B,
                    CreatedBy = 6,
                    Created_Date = DateTime.Now,
                    IsDeleted = false
                };

                var result = _eventService.Add(entity);
                return Json(result.Success
                    ? new { success = true, message = "Etkinlik başarıyla eklendi." }
                    : new { success = false, message = result.Message });
            }

            return Json(new { success = false, message = "Geçersiz veri." });
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var eventResult = _eventService.GetById(id);
            if (!eventResult.Success || eventResult.Data == null)
                return Json(new { success = false, message = "Etkinlik bulunamadı." });

            var entity = eventResult.Data;
            entity.IsDeleted = true;
            entity.Updated_Date = DateTime.Now;
            entity.UpdatedBy = 6;

            var result = _eventService.Update(entity);
            return result.Success
                ? Json(new { success = true, message = "Etkinlik başarıyla silindi." })
                : Json(new { success = false, message = result.Message });
        }



    }
}
