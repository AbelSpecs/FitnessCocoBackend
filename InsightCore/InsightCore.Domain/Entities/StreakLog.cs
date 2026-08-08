using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PyrosFit.Domain.Entities
{
    [Table("StreakLogs")]
    public class StreakLog
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("StudentId")]
        public int StudentId { get; set; }

        [Column("ActivityTypeId")]
        public short ActivityTypeId { get; set; }

        [Column("ActivityDate")]
        public DateTime ActivityDate { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
