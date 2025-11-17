using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tixxp.Entities.Base;

namespace Tixxp.Entities.Events
{
    [Table("Event")]
    public class EventEntity : BaseEntity
    {
        [Column("ImagePath")]
        public string? ImagePath { get; set; }

        [Column("OpeningTime")]
        public TimeSpan? OpeningTime { get; set; }

        [Column("ClosingTime")]
        public TimeSpan? ClosingTime { get; set; }

        [Column("IsAvailableOnB2C")]
        public bool IsAvailableOnB2C { get; set; }

        [Column("IsAvailableOnB2B")]
        public bool IsAvailableOnB2B { get; set; }
    }
}
