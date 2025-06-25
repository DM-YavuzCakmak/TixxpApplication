using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Sockets;
using System;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.SeasonalPrice;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Entities.Events;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Entities.Session;
using Tixxp.WebApp.Models.TicketSale;
using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Business.Services.Abstract.TicketType;

namespace Tixxp.WebApp.Controllers
{
    public class TicketSaleController : Controller
    {
        private readonly IEventService _eventService;
        private readonly IAgencyService _agencyService;
        private readonly ISeasonalPriceService _seasonalPriceService;
        private readonly ISessionService _sessionService;
        private readonly ITicketTypeService _ticketTypeService;

        public TicketSaleController(IEventService eventService, ISeasonalPriceService seasonalPriceService, ISessionService sessionService, IAgencyService agencyService, ITicketTypeService ticketTypeService)
        {
            _eventService = eventService;
            _seasonalPriceService = seasonalPriceService;
            _sessionService = sessionService;
            _agencyService = agencyService;
            _ticketTypeService = ticketTypeService;
        }

        public IActionResult Index()
        {
            var model = new PaymentResultModel(); // Boş da olsa bir instance verilmeli
            return View(model);
        }

        [HttpGet("GetEvent")]
        public async Task<ActionResult> GetEvent()
        {
            List<EventEntity> events = _eventService.GetAll().Data.ToList();
            return Ok(events);
        }

        [HttpGet("CheckSeaseonPrice/{eventId}")]
        public IActionResult CheckSeaseonPrice(Guid eventId)
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //var claims = identity.Claims;

            //var seasonalPrice = HttpProvider.ReadFromResponse<SeasonalPrice>(HttpProvider.HttpGet(_serviceoptions.Url, "TicketType/GetSeasonalPriceByEvent?eventId=" + eventId, claims.FirstOrDefault(x => x.Type == "Token").Value));
            //if (seasonalPrice == null)
            //{
            //    return Json(new { success = false, responseText = "" });
            //}
            //else
            //{
            //    return Json(new { success = true, responseText = "" });
            //}

            List<SeasonalPriceEntity> seasonalPriceEntities = _seasonalPriceService.GetAll().Data.ToList();
            return Json(new { success = true, responseText = "" });
        }


        [HttpGet("GetSessionsByEventId/{eventId}")]
        public async Task<ActionResult> GetSessionsByEventId(Guid eventId, string? from, string? to)
        {
            List<SessionEntity> sessions = _sessionService.GetAll().Data.ToList();

            return Json(new { success = true, responseText = "", data = sessions });
        }

        [HttpGet("Reservation/CreateReservation/{sessionId}")]
        public async Task<ActionResult> CreateReservation(long sessionId)
        {
            //var sessionModel = _sessionService.GetFirstOrDefault(x => x.Id == sessionId);
            //var eventModel = _eventService.GetFirstOrDefault(x => x.Id == sessionModel.Data.EventId).Data;
            //var agencyModel = _agencyService.GetAll().Data.ToList();
            //List<SelectListItem> items = new();
            //items.Add(new() { Text = "Agency Seçimi Yapılmadı", Value = "00000000-0000-0000-0000-000000000000", Disabled = false, Selected = true });
            //foreach (var item in agencyModel)
            //{
            //    items.Add(new() { Text = item.Name, Value = item.Id.ToString() });
            //}
            //ViewBag.Agency = items;

            //var ticketTypes = _ticketTypeService.GetAll().Data.ToList();

            return PartialView("_reservationDetail");

        }
    }
}
