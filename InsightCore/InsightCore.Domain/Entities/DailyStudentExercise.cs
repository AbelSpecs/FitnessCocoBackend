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

        [Column("CoachNotes")]
        public string? CoachNotes { get; set; }

        [Column("StudentNotes")]
        public string? StudentNotes { get; set; }

        [Column("IsCompleted")]
        public bool IsCompleted { get; set; } = false;

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Propiedad de Navegación: Relación hacia el catálogo maestro de Ejercicios
        [ForeignKey("ExerciseId")]
        public virtual Exercise Exercise { get; set; } = null!;

        // Relación 1:N hacia la nueva tabla de Detalle de Sets
        // Inicializada para evitar NullReferenceException al instanciar la clase
        public virtual ICollection<DailyExerciseSet> DailyExerciseSets { get; set; } = new List<DailyExerciseSet>();
    }
}