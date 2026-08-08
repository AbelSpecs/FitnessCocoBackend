using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PyrosFit.Domain.Entities
{
    [Table("StreakActivityTypes")]
    public class StreakActivityTypeEntity
    {
        [Key]
        [Column("Id")]
        public short Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("Code")]
        public string Code { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [Column("Name")]
        public string Name { get; set; } = null!;

        [Column("Description")]
        public string? Description { get; set; }
    }
}
