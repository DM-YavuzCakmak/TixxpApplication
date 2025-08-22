using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Currency;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.TicketSubType;
using Tixxp.Entities.TicketType;
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

    [ForeignKey(nameof(ReservationId))]
    public virtual ReservationEntity Reservation { get; set; }

    [ForeignKey(nameof(TicketTypeId))]
    public virtual TicketTypeEntity TicketType { get; set; }

    [ForeignKey(nameof(TicketSubTypeId))]
    public virtual TicketSubTypeEntity TicketSubType { get; set; }
}
