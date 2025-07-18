using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Abstract.SessionEventTicketPrice;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using System;
using System.Linq;

namespace Tixxp.WebApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IEventService _eventService;
        private readonly IPriceCategoryService _priceCategoryService;
        private readonly ISessionEventTicketPriceService _sessionEventTicketPriceService;
        private readonly IEventTicketPriceService _eventTicketPriceService;

        public SessionController(
            ISessionService sessionService,
            IEventService eventService,
            IPriceCategoryService priceCategoryService,
            ISessionEventTicketPriceService sessionEventTicketPriceService,
            IEventTicketPriceService eventTicketPriceService)
        {
            _sessionService = sessionService;
            _eventService = eventService;
            _priceCategoryService = priceCategoryService;
            _sessionEventTicketPriceService = sessionEventTicketPriceService;
            _eventTicketPriceService = eventTicketPriceService;
        }

        public IActionResult Index()
        {
            var sessions = _sessionService.GetListWithInclude(
                x => !x.IsDeleted,
                e => e.Event
            ).Data;

            // Tüm SessionTicketPrice kayıtlarını ayrı çekiyoruz (SessionId -> EventTicketPrice -> PriceCategory)
            var sessionTicketPrices = _sessionEventTicketPriceService
                .GetListWithInclude(
                    x => !x.IsDeleted,
                    x => x.EventTicketPrice
                ).Data;

            ViewBag.Events = _eventService.GetList(x => !x.IsDeleted).Data;
            ViewBag.PriceCategories = _priceCategoryService.GetList(x => !x.IsDeleted).Data;
            ViewBag.EventTicketPrices = _eventTicketPriceService.GetList(x => !x.IsDeleted).Data;
            ViewBag.SessionTicketPrices = sessionTicketPrices;

            return View(sessions);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _sessionService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            var eventTicketPriceIds = _sessionEventTicketPriceService
                .GetList(x => x.SessionId == id && !x.IsDeleted)
                .Data.Select(x => x.EventTicketPriceId)
                .ToList();

            return Json(new
            {
                id = result.Data.Id,
                eventId = result.Data.EventId,
                eventDate = result.Data.EventDate.ToString("yyyy-MM-dd"),
                plannedTime = result.Data.PlannedTime.ToString(@"hh\:mm"),
                sessionCapacity = result.Data.SessionCapacity,
                availableOnB2C = result.Data.AvailableOnB2C,
                availableOnB2B = result.Data.AvailableOnB2B,
                isCancelled = result.Data.IsCancelled,
                showEntryStartBefore = result.Data.ShowEntryStartBeforeEventTimeInMinutes,
                showEntryEndAfter = result.Data.ShowEntryEndAfterEventTimeInMinutes,
                eventTicketPriceIds = eventTicketPriceIds
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] SessionEntity model)
        {
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;
            model.CreatedBy = 6;

            var result = _sessionService.AddAndReturn(model);
            if (!result.Success)
                return Json(new { success = false, message = result.Message });

            if (model.SessionEventTicketPrices != null)
            {
                foreach (var item in model.SessionEventTicketPrices)
                {
                    item.SessionId = result.Data.Id;
                    item.Created_Date = DateTime.Now;
                    item.IsDeleted = false;
                    item.CreatedBy = 6;
                    _sessionEventTicketPriceService.Add(item);
                }
            }

            return Json(new { success = true, message = "Seans başarıyla oluşturuldu." });
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

            var updateResult = _sessionService.Update(entity);
            if (!updateResult.Success)
                return Json(new { success = false, message = updateResult.Message });

            // Önce eski bağlantıları soft-delete yap
            var oldLinks = _sessionEventTicketPriceService.GetList(x => x.SessionId == model.Id && !x.IsDeleted).Data;
            foreach (var link in oldLinks)
            {
                link.IsDeleted = true;
                link.Updated_Date = DateTime.Now;
                _sessionEventTicketPriceService.Update(link);
            }

            // Yeni bağlantıları ekle
            if (model.SessionEventTicketPrices != null)
            {
                foreach (var item in model.SessionEventTicketPrices)
                {
                    item.SessionId = model.Id;
                    item.Created_Date = DateTime.Now;
                    item.IsDeleted = false;
                    _sessionEventTicketPriceService.Add(item);
                }
            }

            return Json(new { success = true, message = "Güncelleme başarılı." });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _sessionService.GetFirstOrDefault(x => x.Id == id);
            if (!result.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            result.Data.IsDeleted = true;
            var deleteResult = _sessionService.Update(result.Data);

            // SessionEventTicketPrice kayıtlarını da soft-delete et
            var related = _sessionEventTicketPriceService.GetList(x => x.SessionId == id && !x.IsDeleted).Data;
            foreach (var item in related)
            {
                item.IsDeleted = true;
                item.Updated_Date = DateTime.Now;
                _sessionEventTicketPriceService.Update(item);
            }

            return Json(new { success = deleteResult.Success, message = deleteResult.Message });
        }
    }
}
