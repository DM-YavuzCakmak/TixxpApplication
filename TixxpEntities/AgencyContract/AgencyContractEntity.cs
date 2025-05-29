using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.AgencyContract;

[Table("AgencyContract")]
public class AgencyContractEntity : BaseEntity
{
    [Column("AgencyId")]
    public long AgencyId { get; set; }

    [Column("PriceCategoryId")]
    public long PriceCategoryId { get; set; }
}
