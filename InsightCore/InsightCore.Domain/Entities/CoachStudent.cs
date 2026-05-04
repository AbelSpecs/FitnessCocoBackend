using InsightCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("CoachStudents")]
    public class CoachStudent 
    {
        [Key]
        [Column("CoachId", Order = 1)]
        public int CoachId { get; set; }

        [Key]
        [Column("StudentId", Order = 2)]
        public int StudentId { get; set; }

        [Required]
        [Column("AssignedAt")]
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        [Column("Status")]
        public string Status { get; set; } = "Active";

        // Propiedades de navegación (Opcional para EF Core)
        // [ForeignKey("CoachId")]
        // public virtual Coach Coach { get; set; } = null!;

        // [ForeignKey("StudentId")]
        // public virtual Student Student { get; set; } = null!;
    }

}
