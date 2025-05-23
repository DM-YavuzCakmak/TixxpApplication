using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.Product;

namespace Tixxp.Entities.ProductSaleDetail;

[Table("ProductSaleDetail")]
public class ProductSaleDetailEntity : BaseEntity
{
    [Column("ProductSaleId")]
    public long ProductSaleId { get; set; }

    [Column("ProductId")]
    public long ProductId { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity Product { get; set; }
}
 