using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Product;

namespace Tixxp.Entities.ProductPrice;

[Table("ProductPrice")]
public class ProductPriceEntity : BaseEntity
{
    [Column("ProductId")]
    public long ProductId { get; set; }

    [Column("CurrencyTypeId")]
    public int CurrencyTypeId { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }

    [Column("VatRate")]
    public int VatRate { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity Product { get; set; }
}
