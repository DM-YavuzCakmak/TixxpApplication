using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Channel;
using Tixxp.Entities.Language;

namespace Tixxp.Entities.ChannelTranslation;

public class ChannelTranslationEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("LanguageId")]
    public long LanguageId { get; set; }

    [Column("ChannelId")]
    public long ChannelId { get; set; }

    [ForeignKey(nameof(LanguageId))]
    public virtual LanguageEntity Language { get; set; }

    [ForeignKey(nameof(ChannelId))]
    public virtual ChannelEntity Channel { get; set; }
}
