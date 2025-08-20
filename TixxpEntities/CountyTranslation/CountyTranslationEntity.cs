using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.County;

namespace Tixxp.Entities.CountyTranslation;

[Table("CountyTranslation")]
public class CountyTranslationEntity : BaseEntity
{
    [Column("CountyId")]
    public long CountyId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(CountyId))]
    public virtual CountyEntity County { get; set; }
}
