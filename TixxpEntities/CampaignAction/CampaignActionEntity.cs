using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.CampaignAction;

[Table("CampaignAction")]
public class CampaignActionEntity : BaseEntity
{
    [Column("CampaignId")]
    public long CampaignId { get; set; }

    [Column("ActionType")]
    public string ActionType { get; set; }

    [Column("TargetType")]
    public string TargetType { get; set; }

    [Column("TargetId")]
    public long? TargetId { get; set; }

    [Column("Value")]
    public decimal Value { get; set; }

    [Column("ExtraValue")]
    public string? ExtraValue { get; set; }
}
