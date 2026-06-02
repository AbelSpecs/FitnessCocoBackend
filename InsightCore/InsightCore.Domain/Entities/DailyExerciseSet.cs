using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("DailyExerciseSets")]
    public class DailyExerciseSet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [Column("DailyStudentExerciseId")]
        public int DailyStudentExerciseId { get; set; }

        [Required]
        [Column("SetNumber")]
        public int SetNumber { get; set; }

        // --- Planificación del Coach ---

        [Required]
        [Column("TargetReps")]
        public int TargetReps { get; set; }

        [Required]
        [Column("TargetWeight", TypeName = "decimal(5,2)")]
        public decimal TargetWeight { get; set; }

        [MaxLength(30)]
        [Column("RestTime")]
        public string? RestTime { get; set; }

        // --- Ejecución del Alumno ---

        [Column("ActualReps")]
        public int? ActualReps { get; set; }

        [Column("ActualWeight", TypeName = "decimal(5,2)")]
        public decimal? ActualWeight { get; set; }

        [Required]
        [Column("IsAchieved")]
        public bool IsAchieved { get; set; } = false;

        // Propiedad de Navegación inversa hacia la Cabecera
        [ForeignKey("DailyStudentExerciseId")]
        public virtual DailyStudentExercise DailyStudentExercise { get; set; } = null!;
    }
}