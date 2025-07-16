using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.Events;
using Tixxp.Entities.PriceCategory;
using Tixxp.Entities.Role;
using Tixxp.Entities.TicketType;

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

    [ForeignKey(nameof(PriceCategoryId))]
    public virtual PriceCategoryEntity PriceCategory { get; set; }

    [ForeignKey(nameof(EventId))]
    public virtual EventEntity Event { get; set; }

    [ForeignKey(nameof(TicketTypeId))]
    public virtual TicketTypeEntity TicketType { get; set; }

    [ForeignKey(nameof(CurrencyTypeId))]
    public virtual CurrencyTypeEntity CurrencyType { get; set; }
}
