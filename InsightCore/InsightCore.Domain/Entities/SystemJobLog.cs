using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace InsightCore.Domain.Entities
{
    [Table("SystemJobLogs")]
    public class SystemJobLog
    {
        public string Period { get; set; } = default!;
        public string JobName { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }
        public string? Notes { get; set; }
    }
}
