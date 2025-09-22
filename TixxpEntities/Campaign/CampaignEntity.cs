using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Campaign;

[Table("Campaign")]
public class CampaignEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    [Column("StartDate")]
    public DateTime StartDate { get; set; }

    [Column("EndDate")]
    public DateTime EndDate { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }
}

