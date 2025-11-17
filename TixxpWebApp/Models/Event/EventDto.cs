namespace Tixxp.WebApp.Models.Event
{
    public class EventDto
    {
        public long Id { get; set; }
        public string? StartTime { get; set; } // string al (örneğin "12:30")
        public string? EndTime { get; set; }
        public int? DurationInMinutes { get; set; }
        public bool IsAvailableOnB2C { get; set; }
        public bool IsAvailableOnB2B { get; set; }
        public List<EventTranslationInputDto> Translations { get; set; } = new();
    }

    public class EventTranslationInputDto
    {
        public string? LanguageCode { get; set; }
        public string? Name { get; set; }
    }
}
