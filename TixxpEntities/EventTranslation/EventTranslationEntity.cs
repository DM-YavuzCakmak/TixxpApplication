using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.Language;

namespace Tixxp.Entities.EventTranslation;

[Table("EventTranslation")]
public class EventTranslationEntity : BaseEntity
{
    [Column("EventId")]
    public long EventId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual EventEntity Event { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}

