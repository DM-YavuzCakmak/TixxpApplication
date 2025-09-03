using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.TicketStatus;

namespace Tixxp.Entities.TicketStatusTranslation;

[Table("TicketStatusTranslation")]
public class TicketStatusTranslationEntity : BaseEntity
{
    [Column("TicketStatusId")]
    public long TicketStatusId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }


    [ForeignKey(nameof(TicketStatusId))]
    public virtual TicketStatusEntity TicketStatus { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}
