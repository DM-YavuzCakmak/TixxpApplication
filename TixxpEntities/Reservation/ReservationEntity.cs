using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Channel;
using Tixxp.Entities.Currency;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Entities.SessionStatus;

namespace Tixxp.Entities.Reservation;

[Table("Reservation")]
public class ReservationEntity : BaseEntity
{
    [Column("CurrencyId")]
    public long CurrencyId { get; set; }

    [Column("StatusId")]
    public long StatusId { get; set; }

    [Column("ChannelId")]
    public long ChannelId { get; set; }

    [Column("TotalPrice")]
    public decimal? TotalPrice { get; set; }

    [Column("PaidPrice")]
    public decimal? PaidPrice { get; set; }

    [Column("ChangePrice")]
    public decimal? ChangePrice { get; set; }

    [Column("TotalTicket")]
    public int? TotalTicket { get; set; }

    [Column("IsInvoiced")]
    public bool? IsInvoiced { get; set; }


    [ForeignKey(nameof(CurrencyId))]
    public virtual CurrencyEntity Currency { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual ReservationStatusEntity Status { get; set; }

    [ForeignKey(nameof(ChannelId))]
    public virtual ChannelEntity Channel { get; set; }
}
