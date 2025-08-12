using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Events;
using Tixxp.WebApp.Models.Event;

namespace Tixxp.WebApp.Controllers;

[Authorize]
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
        var list = result.Success ? (result.Data ?? new List<EventEntity>()) : new List<EventEntity>();
        return View(list);
    }

    [HttpGet]
    public IActionResult GetById(long id)
    {
        var result = _eventService.GetById(id);
        if (!result.Success || result.Data == null)
            return NotFound(new { success = false, message = "Etkinlik bulunamadı." });

        return Ok(result.Data);
    }

    [HttpPost]
    public IActionResult Create([FromBody] EventDto dto)
    {
        if (dto is null)
            return BadRequest(new { success = false, message = "Geçersiz veri." });

        var entity = new EventEntity
        {
            Name = dto.Name,
            OpeningTime = ParseTime(dto.StartTime),
            ClosingTime = ParseTime(dto.EndTime),
            IsAvailableOnB2C = dto.IsAvailableOnB2C,
            IsAvailableOnB2B = dto.IsAvailableOnB2B,
            CreatedBy = CurrentUserId ?? 0,
            Created_Date = DateTime.UtcNow,
            IsDeleted = false
        };

        var result = _eventService.Add(entity);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = "Etkinlik başarıyla eklendi." });
    }

    [HttpPost]
    public IActionResult Update([FromBody] EventDto dto)
    {
        if (dto is null || dto.Id <= 0)
            return BadRequest(new { success = false, message = "Geçersiz veri." });

        var existing = _eventService.GetById(dto.Id);
        if (!existing.Success || existing.Data is null)
            return NotFound(new { success = false, message = "Etkinlik bulunamadı." });

        var entity = existing.Data;
        ApplyDto(entity, dto);
        entity.UpdatedBy = CurrentUserId ?? 0;
        entity.Updated_Date = DateTime.UtcNow;

        var result = _eventService.Update(entity);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = "Etkinlik başarıyla güncellendi." });
    }

    [HttpPost]
    public IActionResult Delete(long id)
    {
        var existing = _eventService.GetById(id);
        if (!existing.Success || existing.Data is null)
            return NotFound(new { success = false, message = "Etkinlik bulunamadı." });

        var entity = existing.Data;
        entity.IsDeleted = true;
        entity.UpdatedBy = CurrentUserId ?? 0;
        entity.Updated_Date = DateTime.UtcNow;

        var result = _eventService.Update(entity);
        if (!result.Success)
            return BadRequest(new { success = false, message = result.Message });

        return Ok(new { success = true, message = "Etkinlik başarıyla silindi." });
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

    private static void ApplyDto(EventEntity entity, EventDto dto)
    {
        entity.Name = dto.Name;
        entity.OpeningTime = ParseTime(dto.StartTime);
        entity.ClosingTime = ParseTime(dto.EndTime);
        entity.IsAvailableOnB2C = dto.IsAvailableOnB2C;
        entity.IsAvailableOnB2B = dto.IsAvailableOnB2B;
    }
    #endregion
}
