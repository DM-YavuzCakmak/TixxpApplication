using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
namespace Tixxp.Entities.Personnel;

[Table("Personnel")]
public class PersonnelEntity : BaseEntity
{
    [Column("FirstName")]
    public string FirstName { get; set; }

    [Column("LastName")]
    public string LastName { get; set; }

    [Column("UserName")]
    public string UserName { get; set; }

    [Column("Salt")]
    public string Salt { get; set; }

    [Column("Email")]
    public string Email { get; set; }

    [Column("Password")]
    public string Password { get; set; }

    [Column("Phone")]
    public string? Phone { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }

    [Column("NationalIdNumber")]
    public string? NationalIdNumber { get; set; }

    [Column("LoginTypeId")]
    public long LoginType { get; set; } = 0;
}
