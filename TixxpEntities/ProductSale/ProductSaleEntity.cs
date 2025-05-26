using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Counter;
using Tixxp.Entities.Personnel;

namespace Tixxp.Entities.ProductSale;

[Table("ProductSale")]
public class ProductSaleEntity : BaseEntity
{
    [Column("InvoiceTypeId")]
    public long InvoiceTypeId { get; set; }

    [Column("CounterId")]
    public long CounterId { get; set; }


    [Column("Status")]
    public int Status { get; set; } = 1;


    [ForeignKey(nameof(CounterId))]
    public virtual CounterEntity Counter { get; set; }

}
