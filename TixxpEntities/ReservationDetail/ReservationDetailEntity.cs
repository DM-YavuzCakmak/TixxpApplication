using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.ReservationDetail;

[Table("ReservationDetail")]
public class ReservationDetailEntity : BaseEntity
{
    [Column("ReservationId")]
    public long ReservationId { get; set; }

    [Column("TicketTypeId")]
    public long TicketTypeId { get; set; }

    [Column("TicketSubTypeId")]
    public long TicketSubTypeId { get; set; }

    [Column("NumberOfTickets")]
    public int NumberOfTickets { get; set; }
}
