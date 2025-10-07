using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Campaign;
using Tixxp.Entities.Counter;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.ProductSaleStatus;

namespace Tixxp.Entities.ProductSale;

[Table("ProductSale")]
public class ProductSaleEntity : BaseEntity
{
    [Column("InvoiceTypeId")]
    public long InvoiceTypeId { get; set; }

    [Column("CounterId")]
    public long CounterId { get; set; }

    [Column("CampaignId")]
    public long? CampaignId { get; set; }

    [Column("StatusId")]
    public long StatusId { get; set; }

    [ForeignKey(nameof(CounterId))]
    public virtual CounterEntity Counter { get; set; }

    [ForeignKey(nameof(InvoiceTypeId))]
    public virtual InvoiceTypeEntity InvoiceType { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public virtual CampaignEntity? Campaign { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual ProductSaleStatusEntity ProductSaleStatus { get; set; }

}
