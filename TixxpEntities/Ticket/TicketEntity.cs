using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.TicketStatus;

namespace Tixxp.Entities.Ticket;

[Table("Ticket")]
public class TicketEntity : BaseEntity
{
    [Column("ReservationDetailId")]
    public long ReservationDetailId { get; set; }

    [Column("EventId")]
    public long EventId { get; set; }

    [Column("TicketStatusId")]
    public long TicketStatusId { get; set; }

    [Column("CheckInDate")]
    public DateTime? CheckInDate { get; set; }   // Biletle giriş yaptığı an

    [Column("CheckOutDate")]
    public DateTime? CheckOutDate { get; set; }  // Biletle çıkış yaptığı an

    [Column("QrText")]
    public string QrText { get; set; }

    [ForeignKey(nameof(TicketStatusId))]
    public virtual TicketStatusEntity TicketStatus { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual EventEntity Event { get; set; }

    [ForeignKey(nameof(ReservationDetailId))]
    public virtual ReservationDetailEntity ReservationDetail { get; set; }
}
