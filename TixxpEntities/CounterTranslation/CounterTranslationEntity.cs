using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Counter;
using Tixxp.Entities.Language;

namespace Tixxp.Entities.CounterTranslation;

[Table("CounterTranslation")]
public class CounterTranslationEntity : BaseEntity
{
    [Column("CounterId")]
    public long CounterId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }

    [ForeignKey(nameof(CounterId))]
    public virtual CounterEntity Counter { get; set; }
}
