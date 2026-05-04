using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("Exercises")]
    public class Exercise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CoachId")]
        public int? CoachId { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Description")]
        public string? Description { get; set; }

        [MaxLength(50)]
        [Column("MuscleGroup")]
        public string? MuscleGroup { get; set; }

        [MaxLength(255)]
        [Column("VideoUrl")]
        public string? VideoUrl { get; set; }

        [Column("IsCustom")]
        public bool IsCustom { get; set; } = false;

        [Required]
        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
