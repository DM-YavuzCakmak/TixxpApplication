using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Country;

namespace Tixxp.Entities.CountryTranslation;

[Table("CountryTranslation")]
public class CountryTranslationEntity : BaseEntity
{
    [Column("CountryId")]
    public long CountryId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(CountryId))]
    public virtual CountryEntity Country { get; set; }
}
