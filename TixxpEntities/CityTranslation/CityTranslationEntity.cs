using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.City;
namespace Tixxp.Entities.CityTranslation;


[Table("CityTranslation")]
public class CityTranslationEntity : BaseEntity
{
    [Column("CityId")]
    public long CityId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(CityId))]
    public virtual CityEntity City { get; set; }
}
