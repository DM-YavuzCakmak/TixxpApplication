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

    [Column("SecretKey")]
    public string? SecretKey { get; set; }

    [Column("CompanyIdentifier")]
    public string CompanyIdentifier { get; set; }

    [Column("Password")]
    public string Password { get; set; }

    [Column("Phone")]
    public string? Phone { get; set; }

    [Column("IsActive")]
    public bool IsActive { get; set; }

    [Column("NationalIdNumber")]
    public string? NationalIdNumber { get; set; }

    [Column("LoginTypeId")]
    public long LoginTypeId { get; set; } = 0;


    [Column("ParentId")]
    public long? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public virtual PersonnelEntity? Parent { get; set; }

    [Column("ProfilePhotoPath")]
    public string? ProfilePhotoPath { get; set; }

}
