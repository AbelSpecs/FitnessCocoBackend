using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("DailyStudentExercises")]
    public class DailyStudentExercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("CoachId")]
        public int CoachId { get; set; }

        [Required]
        [Column("StudentId")]
        public int StudentId { get; set; }

        [Required]
        [Column("ExerciseId")]
        public int ExerciseId { get; set; }

        [Required]
        [Column("ScheduledDate", TypeName = "date")]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [Column("Sets")]
        public int Sets { get; set; } = 3;

        [MaxLength(50)]
        [Column("Reps")]
        public string? Reps { get; set; }

        [Column("Weight", TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        [MaxLength(30)]
        [Column("RestTime")]
        public string? RestTime { get; set; }

        [Column("CoachNotes")]
        public string? CoachNotes { get; set; }

        [Column("StudentNotes")]
        public string? StudentNotes { get; set; }

        [Column("IsCompleted")]
        public bool IsCompleted { get; set; } = false;

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
