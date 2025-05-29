using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.TicketType;

namespace Tixxp.Entities.TicketSubType;

[Table("TicketSubType")]
public class TicketSubTypeEntity : BaseEntity
{
    [Column("TicketTypeId")]
    public long TicketTypeId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [Column("Description")]
    public string Description { get; set; }


    [ForeignKey(nameof(TicketTypeId))]
    public virtual TicketTypeEntity TicketType { get; set; }
}
