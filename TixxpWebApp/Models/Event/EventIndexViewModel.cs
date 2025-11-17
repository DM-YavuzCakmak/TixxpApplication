using Tixxp.Entities.EventTranslation;
using Tixxp.Entities.Events;
using Tixxp.Entities.Language;

namespace Tixxp.WebApp.Models.Event;

public class EventIndexViewModel
{
    public List<EventListItemViewModel> Events { get; set; } = new();
    public List<LanguageEntity> Languages { get; set; } = new();
}

public class EventListItemViewModel
{
    public EventEntity Event { get; set; }
    public List<EventTranslationEntity> Translations { get; set; } = new();
    public string DisplayName { get; set; } = string.Empty;
}

