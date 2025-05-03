using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Core.Entities;
using Tixxp.Entities.Personnel;

namespace Tixxp.Entities.Base
{
    public class BaseEntity : IEntity
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("Created_By")]
        public long CreatedBy { get; set; }

        [Column("Updated_By")]
        public long? UpdatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual PersonnelEntity CreatedByPersonnel { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual PersonnelEntity UpdatedByPersonnel { get; set; }

        [Column("Created_Date")]
        public DateTime Created_Date { get; set; }

        [Column("Updated_Date")]
        public DateTime? Updated_Date { get; set; }

        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }


    }
}
