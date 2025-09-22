using Tixxp.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tixxp.Entities.CampaignConditionType;


[Table("CampaignConditionType")]
public class CampaignConditionTypeEntity : BaseEntity
{
    [Column("Code")]
    public string Code { get; set; }

    [Column("Description")]
    public string Description { get; set; }
}
