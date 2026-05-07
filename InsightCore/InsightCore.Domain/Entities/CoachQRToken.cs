using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("CoachQRTokens")]
    public class CoachQRToken
    {
        [Key]
        [ForeignKey("CoachId")]
        public int CoachId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Token { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; }

    }
}
