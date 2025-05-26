using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Product;

[Table("Product")]
public class ProductEntity : BaseEntity
{
    [Column("Name")]
    public string Name { get; set; }

    [Column("Code")]
    public string Code { get; set; }

    [Column("ImageFilePath")]
    public string ImageFilePath { get; set; }
}
