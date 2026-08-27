using System;

namespace InsightCore.Application.DTO
{
    public class RateCoachDto
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool WasUpdated { get; set; }
    }
}
