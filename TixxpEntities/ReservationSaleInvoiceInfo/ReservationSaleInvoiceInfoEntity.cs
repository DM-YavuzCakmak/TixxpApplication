using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Base;
using Tixxp.Entities.Currency;
using Tixxp.Entities.Guide;
using Tixxp.Entities.InvoiceType;

namespace Tixxp.Entities.ReservationSaleInvoiceInfo;


[Table("ReservationSaleInvoiceInfo")]
public class ReservationSaleInvoiceInfoEntity : BaseEntity
{
    [Column("ReservationId")]
    public long ReservationId { get; set; }

    [Column("AgencyId")]
    public long? AgencyId { get; set; }

    [Column("GuideId")]
    public long? GuideId { get; set; }

    [Column("PaymentTypeId")]
    public long? PaymentTypeId { get; set; }

    [Column("InvoiceTypeId")]
    public long? InvoiceTypeId { get; set; }

    [Column("CountyId")]
    public long? CountyId { get; set; }

    [Column("BankId")]
    public long? BankId { get; set; }

    #region String Properties

    [Column("Name")]
    public string Name { get; set; }

    [Column("Surname")]
    public string Surname { get; set; }

    [Column("Email")]
    public string Email { get; set; }

    [Column("Phone")]
    public string Phone { get; set; }

    [Column("CompanyName")]
    public string CompanyName { get; set; }

    [Column("TaxNumber")]
    public string TaxNumber { get; set; }

    [Column("TaxOffice")]
    public string TaxOffice { get; set; }

    #endregion

    [ForeignKey(nameof(GuideId))]
    public virtual GuideEntity Guide { get; set; }

    [ForeignKey(nameof(InvoiceTypeId))]
    public virtual InvoiceTypeEntity InvoiceType { get; set; }

    [ForeignKey(nameof(BankId))]
    public virtual BankEntity Bank { get; set; }
}
