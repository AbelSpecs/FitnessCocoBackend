using InsightCore.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Domain.Entities
{
    [Table("UserDeviceTokens")]
    public class UserDeviceToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(500)]
        public string FcmToken { get; set; } = string.Empty;
        [Required]
        [MaxLength(20)]
        public string Platform { get; set; } = "android";
        [MaxLength(100)]
        public string? DeviceModel { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}