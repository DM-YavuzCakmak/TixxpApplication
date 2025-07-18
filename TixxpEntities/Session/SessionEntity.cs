using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.SessionEventTicketPrice;
using System.Collections.Generic;
using System;

namespace Tixxp.Entities.Session
{
    [Table("Session")]
    public class SessionEntity : BaseEntity
    {
        [Column("EventId")]
        public long EventId { get; set; }

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

        [Column("IsDaily")]
        public bool IsDaily { get; set; }

        [Column("IsHourly")]
        public bool IsHourly { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual EventEntity Event { get; set; }

        [NotMapped]
        public virtual ICollection<SessionEventTicketPriceEntity> SessionEventTicketPrices { get; set; }
    }
}
