using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.ProductSale;

[Table("ProductSale")]
public class ProductSaleEntity : BaseEntity
{
    [Column("InvoiceTypeId")]
    public long InvoiceTypeId { get; set; }
}
