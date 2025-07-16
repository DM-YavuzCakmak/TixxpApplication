using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Entities.Session;
using System;
using System.Linq;
using Tixxp.Business.Services.Abstract.PriceCategory;

namespace Tixxp.WebApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IEventService _eventService;
        private readonly IPriceCategoryService _priceCategoryService;

        public SessionController(ISessionService sessionService, IEventService eventService, IPriceCategoryService priceCategoryService)
        {
            _sessionService = sessionService;
            _eventService = eventService;
            _priceCategoryService = priceCategoryService;
        }

        public IActionResult Index()
        {
            var sessions = _sessionService.GetListWithInclude(x => !x.IsDeleted, 
                                                             e => e.Event, 
                                                             p => p.PriceCategory).Data;
            ViewBag.Events = _eventService.GetList(x => !x.IsDeleted).Data;
            ViewBag.PriceCategories = _priceCategoryService.GetList(x => !x.IsDeleted).Data;
            return View(sessions);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _sessionService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                eventId = result.Data.EventId,
                eventDate = result.Data.EventDate.ToString("yyyy-MM-dd"),
                priceCategoryId = result.Data.PriceCategoryId,
                plannedTime = result.Data.PlannedTime.ToString("HH:mm"),
                sessionCapacity = result.Data.SessionCapacity,
                availableOnB2C = result.Data.AvailableOnB2C,
                availableOnB2B = result.Data.AvailableOnB2B,
                isCancelled = result.Data.IsCancelled,
                showEntryStartBefore = result.Data.ShowEntryStartBeforeEventTimeInMinutes,
                showEntryEndAfter = result.Data.ShowEntryEndAfterEventTimeInMinutes
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] SessionEntity model)
        {
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            var result = _sessionService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] SessionEntity model)
        {
            var existing = _sessionService.GetFirstOrDefault(x => x.Id == model.Id);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            var entity = existing.Data;
            entity.EventId = model.EventId;
            entity.EventDate = model.EventDate;
            entity.PlannedTime = model.PlannedTime;
            entity.SessionCapacity = model.SessionCapacity;
            entity.AvailableOnB2C = model.AvailableOnB2C;
            entity.AvailableOnB2B = model.AvailableOnB2B;
            entity.IsCancelled = model.IsCancelled;
            entity.ShowEntryStartBeforeEventTimeInMinutes = model.ShowEntryStartBeforeEventTimeInMinutes;
            entity.ShowEntryEndAfterEventTimeInMinutes = model.ShowEntryEndAfterEventTimeInMinutes;
            entity.Updated_Date = DateTime.Now;

            var result = _sessionService.Update(entity);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _sessionService.GetFirstOrDefault(x => x.Id == id);
            if (!result.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            result.Data.IsDeleted = true;
            var deleteResult = _sessionService.Update(result.Data);
            return Json(new { success = deleteResult.Success, message = deleteResult.Message });
        }
    }
}
