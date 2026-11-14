using System;

namespace InsightCore.Application.DTO.Tickets
{
    public class TicketDto
    {
        public long Id { get; set; }
        public string BugName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAddressed { get; set; }
        public int UserId { get; set; }
        public string? ReporterName { get; set; }
        public string Platform { get; set; } = "android";
        public string? AppVersion { get; set; }
        public string? DeviceModel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? AddressedAt { get; set; }
        public string? ResolutionNotes { get; set; }
    }
}

