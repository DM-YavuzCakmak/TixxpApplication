using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.Product;
using Tixxp.Entities.ProductPrice;
using Tixxp.Entities.ProductSale;

namespace Tixxp.Entities.ProductSaleDetail;

[Table("ProductSaleDetail")]
public class ProductSaleDetailEntity : BaseEntity
{
    [Column("ProductSaleId")]
    public long ProductSaleId { get; set; }

    [Column("ProductId")]
    public long ProductId { get; set; }

    [Column("CurrencyTypeId")]
    public long CurrencyTypeId { get; set; }

    [Column("ProductPriceId")]
    public long? ProductPriceId { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity Product { get; set; }

    [ForeignKey(nameof(ProductPriceId))]
    public virtual ProductPriceEntity ProductPrice { get; set; }

    [ForeignKey(nameof(ProductSaleId))]
    public virtual ProductSaleEntity ProductSale { get; set; }

    [ForeignKey(nameof(CurrencyTypeId))]
    public virtual CurrencyTypeEntity CurrencyType { get; set; }
}
