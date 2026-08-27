using InsightCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("Coaches")]
    public class Coach : BaseEntity
    {
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string? Certifications { get; set; }
        public bool IsVerified { get; set; }

        /// <summary>Años de experiencia del entrenador (default 0).</summary>
        public int YearsOfExperience { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navegación hacia calificaciones recibidas
        public virtual ICollection<CoachRating> Ratings { get; set; } = new List<CoachRating>();
    }
}
