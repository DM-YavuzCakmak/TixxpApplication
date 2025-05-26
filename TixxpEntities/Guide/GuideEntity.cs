using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Guide;

[Table("Guide")]
public class GuideEntity : BaseEntity
{
    [Column("Name")]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Column("NationalIdNumber")]
    [MaxLength(11)]
    public string? NationalIdNumber { get; set; }

    [Column("GsmNumber")]
    [MaxLength(20)]
    public string? GsmNumber { get; set; }

    [Column("IsGsmConfirmed")]
    public bool? IsGsmConfirmed { get; set; }

    [Column("Email")]
    [MaxLength(100)]
    public string? Email { get; set; }

    [Column("IsEmailConfirmed")]
    public bool? IsEmailConfirmed { get; set; }

    [Column("LicenseNumber")]
    [MaxLength(20)]
    public string? LicenseNumber { get; set; }

    [Column("IsActive")]
    public bool? IsActive { get; set; }
}