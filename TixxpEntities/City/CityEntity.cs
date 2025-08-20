using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Country;

namespace Tixxp.Entities.City;

[Table("City")]
public class CityEntity : BaseEntity
{
    [Column("CountryId")]
    public long CountryId { get; set; }


    [ForeignKey(nameof(CountryId))]
    public virtual CountryEntity Country { get; set; }
}
