using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.SessionStatus;
using Tixxp.Entities.SessionType;

namespace Tixxp.Entities.SessionTypeTranslation;

[Table("SessionTypeTranslation")]
public class SessionTypeTranslationEntity : BaseEntity
{
    [Column("SessionTypeId")]
    public long SessionTypeId { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey(nameof(SessionTypeId))]
    public virtual SessionTypeEntity SessionType { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }
}
