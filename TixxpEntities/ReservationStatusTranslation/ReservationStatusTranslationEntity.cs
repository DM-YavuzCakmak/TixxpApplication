using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.ReservationStatus;

namespace Tixxp.Entities.ReservationStatusTranslation;

[Table("ReservationStatusTranslation")]
public class ReservationStatusTranslationEntity : BaseEntity
{
    [Column("ReservationStatusId")]
    public long ReservationStatusId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [ForeignKey(nameof(ReservationStatusId))]
    public virtual ReservationStatusEntity ReservationStatus { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}
