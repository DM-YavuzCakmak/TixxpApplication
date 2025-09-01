using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.TicketType;

namespace Tixxp.Entities.Ticket;

[Table("Ticket")]
public class TicketEntity : BaseEntity
{
    [Column("ReservationDetailId")]
    public long ReservationDetailId { get; set; }

    [Column("EventId")]
    public long EventId { get; set; }   

    [Column("Name")]
    public string Name { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual EventEntity Event { get; set; }

    [ForeignKey(nameof(ReservationDetailId))]
    public virtual ReservationDetailEntity ReservationDetail { get; set; }
}
