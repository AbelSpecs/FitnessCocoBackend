using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("BugName")]
        public string BugName { get; set; } = string.Empty;

        [Required]
        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("IsAddressed")]
        public bool IsAddressed { get; set; } = false;

        [Required]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("Platform")]
        public string Platform { get; set; } = "android";

        [MaxLength(50)]
        [Column("VersionApp")]
        public string? AppVersion { get; set; }

        [MaxLength(100)]
        [Column("DeviceModel")]
        public string? DeviceModel { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("AddressedAt")]
        public DateTime? AddressedAt { get; set; }

        [MaxLength(1000)]
        [Column("ResolutionNotes")]
        public string? ResolutionNotes { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}

