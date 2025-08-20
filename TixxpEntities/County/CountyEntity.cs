using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.City;
using Tixxp.Entities.Country;
namespace Tixxp.Entities.County;

[Table("County")]
public class CountyEntity : BaseEntity
{
    [Column("CityId")]
    public long CityId { get; set; }

    [ForeignKey(nameof(CityId))]
    public virtual CityEntity City { get; set; }
}
