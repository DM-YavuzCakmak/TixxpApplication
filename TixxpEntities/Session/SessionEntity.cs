using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;
using Tixxp.Entities.Events;
using Tixxp.Entities.SessionEventTicketPrice;

namespace Tixxp.Entities.Session
{
    [Table("Session")]
    public class SessionEntity : BaseEntity
    {
        [Column("EventId")]
        public long EventId { get; set; }

        [Column("TypeId")]
        public long? TypeId { get; set; }

        [Column("StatusId")]
        public long? StatusId { get; set; }

        [Column("SessionDate")]
        public DateTime? SessionDate { get; set; }

        [Column("StartTime")]
        public TimeSpan StartTime { get; set; }

        [Column("EndTime")]
        public TimeSpan? EndTime { get; set; } 

        [Column("Capacity")]
        public int Capacity { get; set; }

        [Column("IsAvailableOnB2C")]
        public bool IsAvailableOnB2C { get; set; }

        [Column("IsAvailableOnB2B")]
        public bool IsAvailableOnB2B { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual EventEntity? Event { get; set; }

        [NotMapped]
        public virtual ICollection<SessionEventTicketPriceEntity> SessionEventTicketPrices { get; set; }
            = new List<SessionEventTicketPriceEntity>();
    }
}
