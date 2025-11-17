using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTranslation;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTranslation;
using Tixxp.Entities.EventTicketPrice;

namespace Tixxp.WebApp.Controllers
{
    public class EventTicketPriceController : Controller
    {
        private readonly ICurrentUser _currentUser;


        private readonly IEventService _eventService;
        private readonly IEventTranslationService _eventTranslationService;
        private readonly ILanguageService _languageService;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly IPriceCategoryService _priceCategoryService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IEventTicketPriceService _eventTicketPriceService;

        public EventTicketPriceController(
            IEventService eventService,
            IEventTranslationService eventTranslationService,
            ILanguageService languageService,
            ITicketTypeService ticketTypeService,
            IPriceCategoryService priceCategoryService,
            ICurrencyTypeService currencyTypeService,
            IEventTicketPriceService eventTicketPriceService,
            ICurrentUser currentUser)
        {
            _eventService = eventService;
            _eventTranslationService = eventTranslationService;
            _languageService = languageService;
            _ticketTypeService = ticketTypeService;
            _priceCategoryService = priceCategoryService;
            _currencyTypeService = currencyTypeService;
            _eventTicketPriceService = eventTicketPriceService;
            _currentUser = currentUser;
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

            // Event'leri Translation ile birlikte al
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var fallbackCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var langDr = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langDr.Success ? langDr.Data?.Id : null;

            var eventList = _eventService.GetList(x => !x.IsDeleted);
            var events = (eventList.Success && eventList.Data != null)
                ? eventList.Data
                : Enumerable.Empty<EventEntity>();

            var eventIds = events.Select(e => e.Id).ToList();
            var translationsResult = eventIds.Any()
                ? _eventTranslationService.GetListWithInclude(
                    x => !x.IsDeleted && eventIds.Contains(x.EventId),
                    x => x.Language)
                : new Core.Utilities.Results.Concrete.SuccessDataResult<List<EventTranslationEntity>>(new List<EventTranslationEntity>());

            var translations = (translationsResult.Success && translationsResult.Data != null)
                ? translationsResult.Data.ToList()
                : new List<EventTranslationEntity>();

            // Event isimlerini çözümle ve Dictionary olarak ViewBag'e ekle
            var eventNames = new Dictionary<long, string>();
            foreach (var ev in events)
            {
                var evTranslations = translations.Where(t => t.EventId == ev.Id).ToList();
                var displayName = ResolveEventDisplayName(ev, evTranslations, cultureCode, fallbackCulture, languageId);
                eventNames[ev.Id] = displayName;
            }

            // ViewBag.Events'i Translation ile birlikte doldur
            var eventViewModels = events.Select(ev => new { Id = ev.Id, Name = eventNames.GetValueOrDefault(ev.Id, $"#{ev.Id}") })
                .OrderBy(e => e.Name)
                .ToList();

            ViewBag.Events = eventViewModels;
            ViewBag.EventNames = eventNames; // Model'de event isimlerini kullanmak için
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
            model.CreatedBy = _currentUser.GetRequiredUserId();
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

        // Event display name'i çeviri ile çözümle
        private string ResolveEventDisplayName(EventEntity ev, List<EventTranslationEntity> translations, string cultureCode, string fallbackCulture, long? languageId)
        {
            // Önce mevcut dildeki çeviriyi bul
            var cultureMatch = translations.FirstOrDefault(t =>
                languageId.HasValue && t.LanguageId == languageId.Value);

            // Mevcut culture code ile eşleşen çeviriyi bul
            if (cultureMatch == null)
            {
                cultureMatch = translations.FirstOrDefault(t =>
                    string.Equals(t.Language?.Code, cultureCode, StringComparison.OrdinalIgnoreCase));
            }

            // Fallback culture ile eşleşen çeviriyi bul
            var fallbackMatch = cultureMatch ?? translations.FirstOrDefault(t =>
                string.Equals(t.Language?.Code, fallbackCulture, StringComparison.OrdinalIgnoreCase));

            // Herhangi bir çeviri
            var any = fallbackMatch ?? translations.FirstOrDefault();

            return cultureMatch?.Name
                ?? fallbackMatch?.Name
                ?? any?.Name
                ?? $"#{ev.Id}";
        }
    }
}
