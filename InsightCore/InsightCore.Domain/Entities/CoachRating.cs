using InsightCore.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    /// <summary>
    /// Valoración que un alumno da a su entrenador (1–5 estrellas).
    /// Restricción: un alumno solo puede tener una valoración activa por entrenador (upsert).
    /// </summary>
    [Table("CoachRatings")]
    public class CoachRating : BaseEntity
    {
        [Required]
        [Column("CoachId")]
        public int CoachId { get; set; }

        [Required]
        [Column("StudentId")]
        public int StudentId { get; set; }

        /// <summary>Puntuación entera entre 1 y 5.</summary>
        [Required]
        [Range(1, 5)]
        [Column("Rating")]
        public int Rating { get; set; }

        /// <summary>Comentario opcional del alumno.</summary>
        [MaxLength(1000)]
        [Column("Comment")]
        public string? Comment { get; set; }

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("UpdatedAt")]
        public DateTime? UpdatedAt { get; set; }

        // Propiedades de navegación
        [ForeignKey("CoachId")]
        public virtual Coach? Coach { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student? Student { get; set; }
    }
}
