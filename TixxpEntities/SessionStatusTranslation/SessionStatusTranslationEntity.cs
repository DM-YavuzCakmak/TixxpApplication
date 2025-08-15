using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.SessionStatus;

namespace Tixxp.Entities.SessionStatusTranslation;

[Table("SessionStatusTranslation")]
public class SessionStatusTranslationEntity : BaseEntity
{
    [Column("SessionStatusId")]
    public long SessionStatusId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }


    [Column("Name")]
    public string Name { get; set; }


    [ForeignKey(nameof(SessionStatusId))]
    public virtual SessionStatusEntity SessionStatus { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}
