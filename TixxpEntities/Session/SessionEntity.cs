using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.PriceCategory;
using Tixxp.Entities.Product;

namespace Tixxp.Entities.Session
{
    [Table("Session")]
    public class SessionEntity : BaseEntity
    {
        [Column("EventId")]
        public long EventId { get; set; }

        [Column("PriceCategoryId")]
        public long PriceCategoryId { get; set; }

        [Column("EventDate")]
        public DateTime EventDate { get; set; }

        [Column("PlannedTime")]
        public TimeSpan PlannedTime { get; set; }

        [Column("SessionCapacity")]
        public int SessionCapacity { get; set; }

        [Column("AvailableOnB2C")]
        public bool AvailableOnB2C { get; set; }

        [Column("AvailableOnB2B")]
        public bool AvailableOnB2B { get; set; }

        [Column("IsCancelled")]
        public bool IsCancelled { get; set; }

        [Column("ShowEntryStartBeforeEventTimeInMinutes")]
        public int ShowEntryStartBeforeEventTimeInMinutes { get; set; }

        [Column("ShowEntryEndAfterEventTimeInMinutes")]
        public int ShowEntryEndAfterEventTimeInMinutes { get; set; }


        [ForeignKey(nameof(PriceCategoryId))]
        public virtual PriceCategoryEntity PriceCategory { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual EventEntity Event { get; set; }
    }
}
