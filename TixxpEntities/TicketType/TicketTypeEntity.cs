using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.TicketType;

[Table("TicketType")]
public class TicketTypeEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; } 
}
