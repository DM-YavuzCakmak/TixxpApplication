using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Base;
using Tixxp.Entities.PriceCategory;

namespace Tixxp.Entities.AgencyContract;

[Table("AgencyContract")]
public class AgencyContractEntity : BaseEntity
{
    [Column("AgencyId")]
    public long AgencyId { get; set; }

    [Column("PriceCategoryId")]
    public long PriceCategoryId { get; set; }

    [ForeignKey(nameof(PriceCategoryId))]
    public virtual PriceCategoryEntity PriceCategory { get; set; }

    [NotMapped]
    public virtual AgencyEntity Agency { get; set; }

}
