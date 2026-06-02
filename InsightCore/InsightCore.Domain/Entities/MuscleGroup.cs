using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("MuscleGroups")]
    public class MuscleGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Description")]
        public string? Description { get; set; }

        [MaxLength(255)]
        [Column("ImageUrl")]
        public string? ImageUrl { get; set; }

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relación Inversa (1:N)
        // Permite que desde un grupo muscular puedas acceder a todos sus ejercicios asociados
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}