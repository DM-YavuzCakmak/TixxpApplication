using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.ProductSale;

namespace Tixxp.Entities.ProductSaleInvoiceInfo;

[Table("ProductSaleInvoiceInfo")]
public class ProductSaleInvoiceInfoEntity : BaseEntity
{
    [Column("ProductSaleId")]
    public long ProductSaleId { get; set; }

    [Column("InvoiceTypeId")]
    public long InvoiceTypeId { get; set; }

    [Column("FirstName")]
    public string? FirstName { get; set; }

    [Column("LastName")]
    public string? LastName { get; set; }

    [Column("IdentityNumber")]
    public string? IdentityNumber { get; set; }

    [Column("CompanyName")]
    public string? CompanyName { get; set; }

    [Column("TaxNumber")]
    public string? TaxNumber { get; set; }

    [Column("TaxOffice")]
    public string? TaxOffice { get; set; }

    [Column("CountyId")]
    public long? CountyId { get; set; }

    [ForeignKey(nameof(ProductSaleId))]
    public virtual ProductSaleEntity ProductSale { get; set; }
}
