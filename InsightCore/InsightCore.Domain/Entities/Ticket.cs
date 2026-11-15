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
        [Column("id")]
        public long Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nombre_del_bug")]
        public string BugName { get; set; } = string.Empty;

        [Required]
        [Column("descripcion")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("fue_atacado")]
        public bool IsAddressed { get; set; } = false;

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("plataforma")]
        public string Platform { get; set; } = "android";

        [MaxLength(50)]
        [Column("version_app")]
        public string? AppVersion { get; set; }

        [MaxLength(100)]
        [Column("modelo_dispositivo")]
        public string? DeviceModel { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("addressed_at")]
        public DateTime? AddressedAt { get; set; }

        [MaxLength(1000)]
        [Column("notas_resolucion")]
        public string? ResolutionNotes { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}

