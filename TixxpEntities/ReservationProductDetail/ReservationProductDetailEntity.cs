using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Product;
using Tixxp.Entities.Reservation;

namespace Tixxp.Entities.ReservationProductDetail;

[Table("ReservationProductDetail")]
public class ReservationProductDetailEntity : BaseEntity
{
    [Column("ReservationId")]
    public long ReservationId { get; set; }

    [Column("ProductId")]
    public long ProductId { get; set; }

    [Column("Piece")]
    public int Piece { get; set; }

    [ForeignKey(nameof(ReservationId))]
    public virtual ReservationEntity Reservation { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual ProductEntity Product { get; set; }
}
