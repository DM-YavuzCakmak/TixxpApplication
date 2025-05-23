using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Events
{
    [Table("Event")]
    public class EventEntity : BaseEntity
    {
        [Column("Name")]
        public string? Name { get; set; }

        [Column("StartTime")]
        public TimeSpan? StartTime { get; set; }

        [Column("EndTime")]
        public TimeSpan? EndTime { get; set; }

        [Column("DurationInMinutes")]
        public int? DurationInMinutes { get; set; }

        [Column("IsAvailableOnB2C")]
        public bool IsAvailableOnB2C { get; set; }

        [Column("IsAvailableOnB2B")]
        public bool IsAvailableOnB2B { get; set; }

        [Column("B2CEntryStartOffsetMinutes")]
        public int B2CEntryStartOffsetMinutes { get; set; }

        [Column("B2CEntryEndOffsetMinutes")]
        public int B2CEntryEndOffsetMinutes { get; set; }

        [Column("B2BEntryStartOffsetMinutes")]
        public int B2BEntryStartOffsetMinutes { get; set; }

        [Column("B2BEntryEndOffsetMinutes")]
        public int B2BEntryEndOffsetMinutes { get; set; }
    }
}
