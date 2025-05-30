using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Museum;
namespace Tixxp.Entities.SeasonalPrice;

[Table("SeasonalPrice")]
public class SeasonalPriceEntity : BaseEntity
{
    [Column("MuseumId")]
    public long MuseumId { get; set; }

    [Column("SeasonName")]
    public string SeasonName { get; set; }

    [Column("StartDate")]
    public DateTime StartDate { get; set; }

    [Column("EndDate")]
    public DateTime EndDate { get; set; }


    [NotMapped]
    public virtual MuseumEntity Museum { get; set; }
}
