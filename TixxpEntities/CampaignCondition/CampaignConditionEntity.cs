using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.CampaignCondition;

[Table("CampaignCondition")]
public class CampaignConditionEntity : BaseEntity
{
    [Column("CampaignId")]
    public long CampaignId { get; set; }

    [Column("ConditionTypeId")]
    public long ConditionTypeId { get; set; }

    [Column("Operator")]
    public string Operator { get; set; }

    [Column("Value1")]
    public string? Value1 { get; set; } 

    [Column("Value2")]
    public string? Value2 { get; set; }    
}
