using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.EventTicketPrice;

[Table("EventTicketPrice")]
public class EventTicketPriceEntity : BaseEntity
{
    [Column("EventId")]
    public long EventId { get; set; }

    [Column("TicketTypeId")]
    public long TicketTypeId { get; set; }

    [Column("PriceCategoryId")]
    public long PriceCategoryId { get; set; }

    [Column("CurrencyTypeId")]
    public long CurrencyTypeId { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }
}
