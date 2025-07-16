using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Entities.EventTicketPrice;
using System;

namespace Tixxp.WebApp.Controllers
{
    public class EventTicketPriceController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly IPriceCategoryService _priceCategoryService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IEventTicketPriceService _eventTicketPriceService;

        public EventTicketPriceController(
            IEventService eventService,
            ITicketTypeService ticketTypeService,
            IPriceCategoryService priceCategoryService,
            ICurrencyTypeService currencyTypeService,
            IEventTicketPriceService eventTicketPriceService)
        {
            _eventService = eventService;
            _ticketTypeService = ticketTypeService;
            _priceCategoryService = priceCategoryService;
            _currencyTypeService = currencyTypeService;
            _eventTicketPriceService = eventTicketPriceService;
        }

        public IActionResult Index()
        {
            var prices = _eventTicketPriceService.GetList(x => !x.IsDeleted).Data;

            foreach (var item in prices)
            {
                item.Event = _eventService.GetFirstOrDefault(x => x.Id == item.EventId && !x.IsDeleted).Data;
                item.TicketType = _ticketTypeService.GetFirstOrDefault(x => x.Id == item.TicketTypeId && !x.IsDeleted).Data;
                item.PriceCategory = _priceCategoryService.GetFirstOrDefault(x => x.Id == item.PriceCategoryId && !x.IsDeleted).Data;
                item.CurrencyType = _currencyTypeService.GetFirstOrDefault(x => x.Id == item.CurrencyTypeId && !x.IsDeleted).Data;
            }

            ViewBag.Events = _eventService.GetList(x => !x.IsDeleted).Data;
            ViewBag.TicketTypes = _ticketTypeService.GetList(x => !x.IsDeleted).Data;
            ViewBag.PriceCategories = _priceCategoryService.GetList(x => !x.IsDeleted).Data;
            ViewBag.CurrencyTypes = _currencyTypeService.GetList(x => !x.IsDeleted).Data;

            return View(prices);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _eventTicketPriceService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                eventId = result.Data.EventId,
                ticketTypeId = result.Data.TicketTypeId,
                priceCategoryId = result.Data.PriceCategoryId,
                currencyTypeId = result.Data.CurrencyTypeId,
                price = result.Data.Price
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] EventTicketPriceEntity model)
        {
            model.CreatedBy = 6; // Bu kullanıcı kimliği, örnek amaçlı
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _eventTicketPriceService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] EventTicketPriceEntity model)
        {
            var existing = _eventTicketPriceService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.EventId = model.EventId;
            existing.Data.TicketTypeId = model.TicketTypeId;
            existing.Data.PriceCategoryId = model.PriceCategoryId;
            existing.Data.CurrencyTypeId = model.CurrencyTypeId;
            existing.Data.Price = model.Price;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _eventTicketPriceService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _eventTicketPriceService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            result.Data.IsDeleted = true;
            var updateResult = _eventTicketPriceService.Update(result.Data);
            return Json(new { success = updateResult.Success, message = updateResult.Message });
        }
    }
}
