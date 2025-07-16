using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.Language;

[Table("Language")]
public class LanguageEntity : BaseEntity
{
    [Column("Code")]
    public string Code { get; set; }

    [Column("Name")]
    public string Name { get; set; }
}
