using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.Session;
namespace Tixxp.Entities.SessionEventTicketPrice;

[Table("SessionEventTicketPrice")]
public class SessionEventTicketPriceEntity : BaseEntity
{
    [Column("SessionId")]
    public long SessionId { get; set; }

    [Column("EventTicketPriceId")]
    public long EventTicketPriceId { get; set; }


    [ForeignKey(nameof(SessionId))]
    public virtual SessionEntity Session { get; set; }

    [ForeignKey(nameof(EventTicketPriceId))]
    public virtual EventTicketPriceEntity EventTicketPrice { get; set; }

}
