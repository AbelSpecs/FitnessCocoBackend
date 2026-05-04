using InsightCore.Domain.Common;
using System;
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
