using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Reservation;

[Table("Reservation")]
public class ReservationEntity : BaseEntity
{
    [Column("SessionId")]
    public long SessionId { get; set; }

    [Column("SeasonalPriceId")]
    public long SeasonalPriceId { get; set; }

    [Column("PaymentTypeId")]
    public long PaymentTypeId { get; set; }

    [Column("InvoiceTypeId")]
    public long InvoiceTypeId { get; set; }

    [Column("AgencyId")]
    public long? AgencyId { get; set; }

    [Column("ChannelId")]
    public long ChannelId { get; set; }

    [Column("NationalityId")]
    public long? NationalityId { get; set; }

    [Column("CurrencyId")]
    public int? CurrencyId { get; set; }

    [Column("BankId")]
    public int? BankId { get; set; }

    [Column("CurrencyRateType")]
    public int? CurrencyRateType { get; set; }

    [Column("StatusId")]
    public long StatusId { get; set; }

    [Column("NumberOfTickets")]
    public int? NumberOfTickets { get; set; }

    [Column("NationalIdNumber")]
    public string? NationalIdNumber { get; set; }

    [Column("CompanyName")]
    public string? CompanyName { get; set; }

    [Column("TaxNumber")]
    public string? TaxNumber { get; set; }

    [Column("TaxOffice")]
    public string? TaxOffice { get; set; }

    [Column("Name")]
    public string? Name { get; set; }

    [Column("Surname")]
    public string? Surname { get; set; }

    [Column("Email")]
    public string? Email { get; set; }

    [Column("Phone")]
    public string? Phone { get; set; }

    [Column("Note")]
    public string? Note { get; set; }

    [Column("CountryId")]
    public int? CountryId { get; set; }

    [Column("CityId")]
    public int? CityId { get; set; }

    [Column("StateId")]
    public int? StateId { get; set; }

    [Column("TotalPrice")]
    public decimal? TotalPrice { get; set; }

    [Column("PaidPrice")]
    public decimal? PaidPrice { get; set; }

    [Column("ChangePrice")]
    public decimal? ChangePrice { get; set; }

    [Column("ReasonForCancellation")]
    public string? ReasonForCancellation { get; set; }

    [Column("UserIdForCancel")]
    public Guid? UserIdForCancel { get; set; }

    [Column("CancelledDate")]
    public DateTime? CancelledDate { get; set; }

    [Column("IsInvoiced")]
    public bool? IsInvoiced { get; set; }

    [Column("CurrencyRate")]
    public decimal? CurrencyRate { get; set; }
}
