using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.SessionEventTicketPrice;
using Tixxp.Business.Services.Abstract.SessionStatus;
using Tixxp.Business.Services.Abstract.SessionType;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;

namespace Tixxp.WebApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        private readonly IEventService _eventService;
        private readonly IPriceCategoryService _priceCategoryService;
        private readonly ISessionEventTicketPriceService _sessionEventTicketPriceService;
        private readonly IEventTicketPriceService _eventTicketPriceService;
        private readonly ILanguageService _languageService;

        private readonly ISessionTypeService _sessionTypeService;
        private readonly ISessionStatusService _sessionStatusService;
        private readonly ISessionTypeTranslationRepository _sessionTypeTranslationRepository;
        private readonly ISessionStatusTranslationRepository _sessionStatusTranslationRepository;

        private readonly IStringLocalizer<SessionController> _stringLocalizer;

        public SessionController(
            ISessionService sessionService,
            IEventService eventService,
            IPriceCategoryService priceCategoryService,
            ISessionEventTicketPriceService sessionEventTicketPriceService,
            IEventTicketPriceService eventTicketPriceService,
            ILanguageService languageService,
            ISessionTypeService sessionTypeService,
            ISessionStatusService sessionStatusService,
            ISessionTypeTranslationRepository sessionTypeTranslationRepository,
            ISessionStatusTranslationRepository sessionStatusTranslationRepository,
            IStringLocalizer<SessionController> stringLocalizer)
        {
            _sessionService = sessionService;
            _eventService = eventService;
            _priceCategoryService = priceCategoryService;
            _sessionEventTicketPriceService = sessionEventTicketPriceService;
            _eventTicketPriceService = eventTicketPriceService;
            _languageService = languageService;
            _sessionTypeService = sessionTypeService;
            _sessionStatusService = sessionStatusService;
            _sessionTypeTranslationRepository = sessionTypeTranslationRepository;
            _sessionStatusTranslationRepository = sessionStatusTranslationRepository;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var sessions = _sessionService.GetListWithInclude(
                x => !x.IsDeleted,
                e => e.Event
            ).Data ?? new List<SessionEntity>();

            // Dil Id
            long? languageId = null;
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            if (langRes.Success) languageId = langRes.Data?.Id;

            // ==== Liste görünümü için isim sözlükleri (yalnızca listede var olan id'lerden) ====
            var typeIds = sessions.Where(s => s.TypeId.HasValue).Select(s => s.TypeId!.Value).Distinct().ToList();
            var statusIds = sessions.Where(s => s.StatusId.HasValue).Select(s => s.StatusId!.Value).Distinct().ToList();

            var typeTrAll = typeIds.Count == 0
                ? new List<Tixxp.Entities.SessionTypeTranslation.SessionTypeTranslationEntity>()
                : (_sessionTypeTranslationRepository.GetList(t => typeIds.Contains(t.SessionTypeId))
                   ?? new List<Tixxp.Entities.SessionTypeTranslation.SessionTypeTranslationEntity>());

            var statusTrAll = statusIds.Count == 0
                ? new List<Tixxp.Entities.SessionStatusTranslation.SessionStatusTranslationEntity>()
                : (_sessionStatusTranslationRepository.GetList(s => statusIds.Contains(s.SessionStatusId))
                   ?? new List<Tixxp.Entities.SessionStatusTranslation.SessionStatusTranslationEntity>());

            string? PickTypeName(long id)
            {
                var list = typeTrAll.Where(x => x.SessionTypeId == id);
                var preferred = (languageId != null) ? list.FirstOrDefault(x => x.LanguageId == languageId) : null;
                return preferred?.Name ?? list.Select(x => x.Name).FirstOrDefault();
            }
            string? PickStatusName(long id)
            {
                var list = statusTrAll.Where(x => x.SessionStatusId == id);
                var preferred = (languageId != null) ? list.FirstOrDefault(x => x.LanguageId == languageId) : null;
                return preferred?.Name ?? list.Select(x => x.Name).FirstOrDefault();
            }

            ViewBag.TypeNames = typeIds.ToDictionary(id => id, id => PickTypeName(id) ?? $"#{id}");
            ViewBag.StatusNames = statusIds.ToDictionary(id => id, id => PickStatusName(id) ?? $"#{id}");

            // ==== Modal dropdown'ları her zaman doldur (Session olmasa bile) ====
            var typeBase = _sessionTypeService.GetList(x => !x.IsDeleted).Data ?? new List<Tixxp.Entities.SessionType.SessionTypeEntity>();
            var statusBase = _sessionStatusService.GetList(x => !x.IsDeleted).Data ?? new List<Tixxp.Entities.SessionStatus.SessionStatusEntity>();

            var typeAllIds = typeBase.Select(t => t.Id).ToList();
            var statusAllIds = statusBase.Select(s => s.Id).ToList();

            var typeAllTr = typeAllIds.Count == 0
                ? new List<Tixxp.Entities.SessionTypeTranslation.SessionTypeTranslationEntity>()
                : (_sessionTypeTranslationRepository.GetList(t => typeAllIds.Contains(t.SessionTypeId))
                   ?? new List<Tixxp.Entities.SessionTypeTranslation.SessionTypeTranslationEntity>());

            var statusAllTr = statusAllIds.Count == 0
                ? new List<Tixxp.Entities.SessionStatusTranslation.SessionStatusTranslationEntity>()
                : (_sessionStatusTranslationRepository.GetList(s => statusAllIds.Contains(s.SessionStatusId))
                   ?? new List<Tixxp.Entities.SessionStatusTranslation.SessionStatusTranslationEntity>());

            string? GetTypeName(long id)
            {
                var list = typeAllTr.Where(x => x.SessionTypeId == id);
                var preferred = (languageId != null) ? list.FirstOrDefault(x => x.LanguageId == languageId) : null;
                return preferred?.Name ?? list.Select(x => x.Name).FirstOrDefault();
            }
            string? GetStatusName(long id)
            {
                var list = statusAllTr.Where(x => x.SessionStatusId == id);
                var preferred = (languageId != null) ? list.FirstOrDefault(x => x.LanguageId == languageId) : null;
                return preferred?.Name ?? list.Select(x => x.Name).FirstOrDefault();
            }

            ViewBag.SessionTypes = typeBase
                .Select(t => new { Id = t.Id, Name = GetTypeName(t.Id) ?? $"Type #{t.Id}" })
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.SessionStatuses = statusBase
                .Select(s => new { Id = s.Id, Name = GetStatusName(s.Id) ?? $"Status #{s.Id}" })
                .OrderBy(x => x.Name)
                .ToList();

            // Diğer lookuplar
            var sessionTicketPrices = _sessionEventTicketPriceService
                .GetListWithInclude(x => !x.IsDeleted, x => x.EventTicketPrice)
                .Data;

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
            if (!result.Success || result.Data == null)
                return BadRequest(new { success = false, message = result.Message ?? _stringLocalizer["sessionController.DATA_NOT_FOUND"].ToString() });

            var eventTicketPriceIds = _sessionEventTicketPriceService
                .GetList(x => x.SessionId == id && !x.IsDeleted)
                .Data?
                .Select(x => x.EventTicketPriceId)
                .ToList() ?? new List<long>();

            var s = result.Data;

            return Json(new
            {
                id = s.Id,
                eventId = s.EventId,
                sessionDate = s.SessionDate?.ToString("yyyy-MM-dd"),
                startTime = s.StartTime.ToString(@"hh\:mm"),
                endTime = s.EndTime?.ToString(@"hh\:mm"),
                capacity = s.Capacity,
                availableOnB2C = s.IsAvailableOnB2C,
                availableOnB2B = s.IsAvailableOnB2B,
                typeId = s.TypeId,
                statusId = s.StatusId,
                eventTicketPriceIds
            });
        }

        // ---- yardımcı parse'lar + Create/Update/Delete aynı kalsın ----
        private static TimeSpan? ParseTime(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (s!.Length == 5) s += ":00";
            return TimeSpan.TryParse(s, out var ts) ? ts : (TimeSpan?)null;
        }

        private static DateTime? ParseDate(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return DateTime.TryParse(s, out var dt) ? dt.Date : (DateTime?)null;
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Create([FromBody] SessionUpsertDto dto)
        {
            if (dto == null) return Json(new { success = false, message = "İstek gövdesi boş." });
            if (dto.EventId <= 0) return Json(new { success = false, message = "Event seçimi zorunludur." });

            var sessionDate = ParseDate(dto.SessionDate);
            if (sessionDate == null) return Json(new { success = false, message = "SessionDate formatı hatalı (yyyy-MM-dd)." });

            var startTs = ParseTime(dto.StartTime);
            if (startTs == null) return Json(new { success = false, message = "StartTime formatı hatalı (HH:mm veya HH:mm:ss)." });

            var endTs = ParseTime(dto.EndTime);

            var entity = new SessionEntity
            {
                EventId = dto.EventId,
                TypeId = dto.TypeId,
                StatusId = dto.StatusId,
                SessionDate = sessionDate,
                StartTime = startTs.Value,
                EndTime = endTs,
                Capacity = dto.Capacity,
                IsAvailableOnB2C = dto.IsAvailableOnB2C,
                IsAvailableOnB2B = dto.IsAvailableOnB2B,
                IsDeleted = false,
                Created_Date = DateTime.Now,
                CreatedBy = 6
            };

            var result = _sessionService.AddAndReturn(entity);
            if (!result.Success || result.Data == null)
                return Json(new { success = false, message = result.Message ?? _stringLocalizer["sessionController.SESSION_COLUD_NOT_BE_CREATED"] });

            var sessionId = result.Data.Id;

            if (dto.EventTicketPriceIds?.Count > 0)
            {
                foreach (var etpId in dto.EventTicketPriceIds.Distinct())
                {
                    _sessionEventTicketPriceService.Add(new SessionEventTicketPriceEntity
                    {
                        SessionId = sessionId,
                        EventTicketPriceId = etpId,
                        Created_Date = DateTime.Now,
                        CreatedBy = 6,
                        IsDeleted = false
                    });
                }
            }

            return Json(new { success = true, message = _stringLocalizer["sessionController.SESSION_CREATED_SUCCESS"], id = sessionId });
        }

        [HttpPost]
        public IActionResult Update([FromBody] SessionEntity model)
        {


            var existing = _sessionService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success || existing.Data == null)
                return Json(new { success = false, message = _stringLocalizer["sessionController.DATA_NOT_FOUND"].ToString() });

            var entity = existing.Data;
            entity.EventId = model.EventId;
            entity.SessionDate = model.SessionDate;
            entity.StartTime = model.StartTime;
            entity.EndTime = model.EndTime;
            entity.Capacity = model.Capacity;
            entity.IsAvailableOnB2C = model.IsAvailableOnB2C;
            entity.IsAvailableOnB2B = model.IsAvailableOnB2B;
            entity.TypeId = model.TypeId;
            entity.StatusId = model.StatusId;
            entity.Updated_Date = DateTime.Now;

            var updateResult = _sessionService.Update(entity);
            if (!updateResult.Success)
                return Json(new { success = false, message = updateResult.Message ?? _stringLocalizer["sessionController.UPDATE_FAILED"].ToString() });

            var oldLinks = _sessionEventTicketPriceService
                .GetList(x => x.SessionId == entity.Id && !x.IsDeleted)
                .Data ?? new List<SessionEventTicketPriceEntity>();

            foreach (var link in oldLinks)
            {
                link.IsDeleted = true;
                link.Updated_Date = DateTime.Now;
                _sessionEventTicketPriceService.Update(link);
            }

            if (model.SessionEventTicketPrices != null && model.SessionEventTicketPrices.Count > 0)
            {
                foreach (var item in model.SessionEventTicketPrices.GroupBy(x => x.EventTicketPriceId).Select(g => g.First()))
                {
                    item.SessionId = entity.Id;
                    item.Created_Date = DateTime.Now;
                    item.IsDeleted = false;
                    item.CreatedBy = 6;
                    _sessionEventTicketPriceService.Add(item);
                }
            }

            return Json(new { success = true, message = _stringLocalizer["sessionController.UPDATE_SUCCESS"] });
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult CreateBulk([FromBody] SessionBulkDto dto)
        {
            if (dto == null) return Json(new { success = false, message = "İstek boş." });
            if (dto.EventId <= 0) return Json(new { success = false, message = "Etkinlik zorunlu." });
            if (dto.Dates.Count == 0 || dto.StartTimes.Count == 0)
                return Json(new { success = false, message = "En az bir tarih ve saat ekleyin." });

            var parsedDates = dto.Dates
                .Select(s => DateTime.TryParse(s, out var d) ? d.Date : (DateTime?)null)
                .Where(d => d != null).Select(d => d!.Value).Distinct().ToList();

            if (parsedDates.Count == 0)
                return Json(new { success = false, message = "Tarih formatı hatalı." });

            TimeSpan? endTs = null;
            if (!string.IsNullOrWhiteSpace(dto.EndTime))
            {
                var e = dto.EndTime!.Length == 5 ? dto.EndTime + ":00" : dto.EndTime;
                if (TimeSpan.TryParse(e, out var ts)) endTs = ts;
            }

            var startTsList = dto.StartTimes
                .Select(s => s?.Length == 5 ? s + ":00" : s)
                .Select(s => TimeSpan.TryParse(s, out var ts) ? (TimeSpan?)ts : null)
                .Where(ts => ts != null).Select(ts => ts!.Value).Distinct().ToList();

            if (startTsList.Count == 0)
                return Json(new { success = false, message = "Saat formatı hatalı." });

            var created = 0;

            foreach (var date in parsedDates)
            {
                foreach (var st in startTsList)
                {
                    var entity = new SessionEntity
                    {
                        EventId = dto.EventId,
                        TypeId = dto.TypeId,
                        StatusId = dto.StatusId,
                        SessionDate = date,
                        StartTime = st,
                        EndTime = endTs,
                        Capacity = dto.Capacity,
                        IsAvailableOnB2C = dto.IsAvailableOnB2C,
                        IsAvailableOnB2B = dto.IsAvailableOnB2B,
                        IsDeleted = false,
                        Created_Date = DateTime.Now,
                        CreatedBy = 6
                    };

                    var res = _sessionService.AddAndReturn(entity);
                    if (!res.Success || res.Data == null) continue;

                    var sid = res.Data.Id;
                    created++;

                    if (dto.EventTicketPriceIds?.Count > 0)
                    {
                        foreach (var etpId in dto.EventTicketPriceIds.Distinct())
                        {
                            _sessionEventTicketPriceService.Add(new SessionEventTicketPriceEntity
                            {
                                SessionId = sid,
                                EventTicketPriceId = etpId,
                                Created_Date = DateTime.Now,
                                CreatedBy = 6,
                                IsDeleted = false
                            });
                        }
                    }
                }
            }

            return Json(new { success = true, message = $"{created} seans oluşturuldu." });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _sessionService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success || result.Data == null)
                return Json(new { success = false, message = _stringLocalizer["sessionController.DATA_NOT_FOUND"].ToString() });

            var entity = result.Data;
            entity.IsDeleted = true;
            entity.Updated_Date = DateTime.Now;

            var deleteResult = _sessionService.Update(entity);

            var related = _sessionEventTicketPriceService.GetList(x => x.SessionId == id && !x.IsDeleted).Data
                          ?? new List<SessionEventTicketPriceEntity>();

            foreach (var item in related)
            {
                item.IsDeleted = true;
                item.Updated_Date = DateTime.Now;
                _sessionEventTicketPriceService.Update(item);
            }

            return Json(new
            {
                success = deleteResult.Success,
                message = deleteResult.Message ?? (deleteResult.Success ? _stringLocalizer["sessionController.DELETED"] : _stringLocalizer["sessionController.DELETION_FAILED"])
            });
        }
    }

    internal static class LinqExt
    {
        public static IEnumerable<T> GroupByFirst<T, TKey>(this IEnumerable<T> src, Func<T, TKey> key)
            => src.GroupBy(key).Select(g => g.First());
    }

    public class SessionUpsertDto
    {
        public long EventId { get; set; }
        public long? TypeId { get; set; }
        public long? StatusId { get; set; }
        public string SessionDate { get; set; } = ""; 
        public string StartTime { get; set; } = "";   
        public string? EndTime { get; set; }          
        public int Capacity { get; set; }
        public bool IsAvailableOnB2C { get; set; }
        public bool IsAvailableOnB2B { get; set; }
        public List<long> EventTicketPriceIds { get; set; } = new();
    }

    public class SessionBulkDto
    {
        public long EventId { get; set; }
        public long? TypeId { get; set; }
        public long? StatusId { get; set; }
        public List<string> Dates { get; set; } = new();      // "yyyy-MM-dd"
        public List<string> StartTimes { get; set; } = new(); // "HH:mm[:ss]"
        public string? EndTime { get; set; }                  // ortak bitiş
        public int Capacity { get; set; }
        public bool IsAvailableOnB2C { get; set; }
        public bool IsAvailableOnB2B { get; set; }
        public List<long> EventTicketPriceIds { get; set; } = new();
    }
}
