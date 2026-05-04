using InsightCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("Students")]
    public class Student : BaseEntity
    {

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        // Información Física
        [Column("Weight", TypeName = "decimal(5,2)")]
        public decimal? Weight { get; set; }

        [Column("Height", TypeName = "decimal(3,2)")]
        public decimal? Height { get; set; }

        [Column("BodyFatPercentage", TypeName = "decimal(4,2)")]
        public decimal? BodyFatPercentage { get; set; }

        // Historial de Salud y Objetivos
        [MaxLength(50)]
        [Column("FitnessGoal")]
        public string? FitnessGoal { get; set; }

        [MaxLength(30)]
        [Column("ActivityLevel")]
        public string? ActivityLevel { get; set; }

        [Column("MedicalConditions")]
        public string? MedicalConditions { get; set; }

        [Column("Allergies")]
        public string? Allergies { get; set; }

        [MaxLength(30)]
        [Column("FitnessExperience")]
        public string? FitnessExperience { get; set; }

        // Notas y Auditoría
        [Column("GeneralNotes")]
        public string? GeneralNotes { get; set; }

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        // Propiedad de navegación (Opcional, si usas EF Core para relacionarlo con User)
        // public virtual User User { get; set; } = null!;
    }
}
