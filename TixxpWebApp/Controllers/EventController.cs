using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTranslation;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.EventTranslation;
using Tixxp.Entities.Events;
using Tixxp.Entities.Language;
using Tixxp.WebApp.Models.Event;

namespace Tixxp.WebApp.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IEventTranslationService _eventTranslationService;
    private readonly ILanguageService _languageService;
    private readonly IStringLocalizer<EventController> _stringLocalizer;

    public EventController(
        IEventService eventService,
        IEventTranslationService eventTranslationService,
        ILanguageService languageService,
        IStringLocalizer<EventController> stringLocalizer)
    {
        _eventService = eventService;
        _eventTranslationService = eventTranslationService;
        _languageService = languageService;
        _stringLocalizer = stringLocalizer;
    }

    public IActionResult Index()
    {
        var eventsResult = _eventService.GetList(x => !x.IsDeleted);
        var translationsResult = _eventTranslationService.GetListWithInclude(x => !x.IsDeleted, x => x.Language);
        var languagesResult = _languageService.GetList(x => !x.IsDeleted);

        var events = eventsResult.Success && eventsResult.Data != null
            ? eventsResult.Data
            : new List<EventEntity>();

        var translations = translationsResult.Success && translationsResult.Data != null
            ? translationsResult.Data
            : new List<EventTranslationEntity>();

        var languages = languagesResult.Success && languagesResult.Data != null
            ? languagesResult.Data.OrderBy(l => l.Name).ToList()
            : new List<LanguageEntity>();

        var currentCulture = CultureInfo.CurrentUICulture.Name;
        var fallbackCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        var viewModel = new EventIndexViewModel
        {
            Languages = languages,
            Events = events.Select(ev =>
            {
                var tr = translations.Where(t => t.EventId == ev.Id).ToList();
                return new EventListItemViewModel
                {
                    Event = ev,
                    Translations = tr,
                    DisplayName = ResolveDisplayName(ev, tr, currentCulture, fallbackCulture)
                };
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult GetById(long id)
    {
        var result = _eventService.GetById(id);
        if (!result.Success || result.Data == null)
            return NotFound(new { success = false, message = _stringLocalizer["eventController.GET_BY_ID.EVENT_NOT_FOUND"].ToString() });

        var translations = _eventTranslationService.GetListWithInclude(
            x => x.EventId == id && !x.IsDeleted,
            x => x.Language);

        var payload = new
        {
            id = result.Data.Id,
            openingTime = result.Data.OpeningTime?.ToString(@"hh\:mm"),
            closingTime = result.Data.ClosingTime?.ToString(@"hh\:mm"),
            isAvailableOnB2C = result.Data.IsAvailableOnB2C,
            isAvailableOnB2B = result.Data.IsAvailableOnB2B,
            translations = (translations.Success && translations.Data != null)
        ? translations.Data.Select(t => new
        {
            languageCode = t.Language?.Code,
            name = t.Name
        }).ToArray()
        : Array.Empty<object>()
        };

        return Ok(payload);
    }

    [HttpPost]
    public IActionResult Create([FromBody] EventDto dto)
    {
        if (dto is null)
            return BadRequest(new { success = false, message = _stringLocalizer["eventController.CREATE.INVALID_DATA"].ToString() });

        var translationPayload = NormalizeTranslations(dto);
        if (!translationPayload.Any(t => !string.IsNullOrWhiteSpace(t.Name)))
            return BadRequest(new { success = false, message = _stringLocalizer["eventController.VALIDATION.TRANSLATION_REQUIRED"].ToString() });

        var entity = new EventEntity
        {
            OpeningTime = ParseTime(dto.StartTime),
            ClosingTime = ParseTime(dto.EndTime),
            IsAvailableOnB2C = dto.IsAvailableOnB2C,
            IsAvailableOnB2B = dto.IsAvailableOnB2B,
            CreatedBy = CurrentUserId ?? 0,
            Created_Date = DateTime.UtcNow,
            IsDeleted = false
        };

        var result = _eventService.AddAndReturn(entity);
        if (!result.Success || result.Data == null)
            return BadRequest(new { success = false, message = result.Message });

        UpsertTranslations(result.Data.Id, translationPayload);

        return Ok(new { success = true, message = _stringLocalizer["eventController.CREATE.EVENT_ADDED_SUCCESS"].ToString() });
    }

    [HttpPost]
    public IActionResult Update([FromBody] EventDto dto)
    {
        if (dto is null || dto.Id <= 0)
            return BadRequest(new { success = false, message = _stringLocalizer["eventController.CREATE.INVALID_DATA"].ToString() });

        var existing = _eventService.GetById(dto.Id);
        if (!existing.Success || existing.Data is null)
            return NotFound(new { success = false, message = _stringLocalizer["eventController.GET_BY_ID.EVENT_NOT_FOUND"].ToString() });

        var translationPayload = NormalizeTranslations(dto);
        if (!translationPayload.Any(t => !string.IsNullOrWhiteSpace(t.Name)))
            return BadRequest(new { success = false, message = _stringLocalizer["eventController.VALIDATION.TRANSLATION_REQUIRED"].ToString() });

        var entity = existing.Data;
        ApplyDto(entity, dto);
        entity.UpdatedBy = CurrentUserId ?? 0;
        entity.Updated_Date = DateTime.UtcNow;

        var result = _eventService.Update(entity);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        UpsertTranslations(entity.Id, translationPayload);

        return Ok(new { success = true, message = _stringLocalizer["eventController.UPDATE.EVENT_SUCCESS"].ToString() });
    }

    [HttpPost]
    public IActionResult Delete(long id)
    {
        var existing = _eventService.GetById(id);
        if (!existing.Success || existing.Data is null)
            return NotFound(new { success = false, message = _stringLocalizer["eventController.GET_BY_ID.EVENT_NOT_FOUND"].ToString() });

        var entity = existing.Data;
        entity.IsDeleted = true;
        entity.UpdatedBy = CurrentUserId ?? 0;
        entity.Updated_Date = DateTime.UtcNow;

        var result = _eventService.Update(entity);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = _stringLocalizer["eventController.DELETED.EVENT_SUCCESS"].ToString() });
    }

    #region Helpers
    private long? CurrentUserId
    {
        get
        {
            var idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.TryParse(idStr, out var id) ? id : null;
        }
    }

    private static TimeSpan? ParseTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        return TimeSpan.TryParse(input, out var ts) ? ts : null;
    }

    private static List<EventTranslationInputDto> NormalizeTranslations(EventDto dto)
    {
        return (dto.Translations ?? new List<EventTranslationInputDto>())
            .Where(t => !string.IsNullOrWhiteSpace(t.LanguageCode))
            .Select(t => new EventTranslationInputDto
            {
                LanguageCode = t.LanguageCode?.Trim(),
                Name = t.Name?.Trim()
            })
            .ToList();
    }

    private static void ApplyDto(EventEntity entity, EventDto dto)
    {
        entity.OpeningTime = ParseTime(dto.StartTime);
        entity.ClosingTime = ParseTime(dto.EndTime);
        entity.IsAvailableOnB2C = dto.IsAvailableOnB2C;
        entity.IsAvailableOnB2B = dto.IsAvailableOnB2B;
    }

    private static string ResolveDisplayName(EventEntity ev, List<EventTranslationEntity> translations, string cultureCode, string fallbackCulture)
    {
        var cultureMatch = translations.FirstOrDefault(t =>
            string.Equals(t.Language?.Code, cultureCode, StringComparison.OrdinalIgnoreCase));

        var fallbackMatch = translations.FirstOrDefault(t =>
            string.Equals(t.Language?.Code, fallbackCulture, StringComparison.OrdinalIgnoreCase));

        var any = translations.FirstOrDefault();

        return cultureMatch?.Name
            ?? fallbackMatch?.Name
            ?? any?.Name
            ?? $"#{ev.Id}";
    }

    private void UpsertTranslations(long eventId, List<EventTranslationInputDto> translations)
    {
        if (translations == null || translations.Count == 0)
            return;

        var languageLookup = GetLanguageLookup();
        if (languageLookup.Count == 0)
            return;

        var now = DateTime.UtcNow;
        var actorId = CurrentUserId ?? 0;

        foreach (var translation in translations)
        {
            var codeKey = translation.LanguageCode?.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(codeKey) || !languageLookup.TryGetValue(codeKey, out var language))
                continue;

            var existing = _eventTranslationService.GetFirstOrDefault(x => x.EventId == eventId && x.LanguageId == language.Id);
            var hasValue = !string.IsNullOrWhiteSpace(translation.Name);

            if (existing.Success && existing.Data != null)
            {
                var entity = existing.Data;
                entity.UpdatedBy = actorId;
                entity.Updated_Date = now;
                entity.IsDeleted = !hasValue;
                if (hasValue)
                {
                    entity.Name = translation.Name!;
                }

                _eventTranslationService.Update(entity);
            }
            else if (hasValue)
            {
                var newTranslation = new EventTranslationEntity
                {
                    EventId = eventId,
                    LanguageId = language.Id,
                    Name = translation.Name!,
                    CreatedBy = actorId,
                    Created_Date = now,
                    IsDeleted = false
                };
                _eventTranslationService.Add(newTranslation);
            }
        }
    }

    private Dictionary<string, LanguageEntity> GetLanguageLookup()
    {
        var result = _languageService.GetList(x => !x.IsDeleted);
        if (!result.Success || result.Data == null)
            return new Dictionary<string, LanguageEntity>();

        return result.Data
            .Where(l => !string.IsNullOrWhiteSpace(l.Code))
            .ToDictionary(l => l.Code.Trim().ToLowerInvariant(), l => l);
    }
    #endregion
}
