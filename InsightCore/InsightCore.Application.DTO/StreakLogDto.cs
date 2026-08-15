using System;

namespace InsightCore.Application.DTO
{
    public class StreakLogDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public short ActivityTypeId { get; set; }
        public string ActivityTypeCode { get; set; } = string.Empty;
        public string ActivityTypeName { get; set; } = string.Empty;
        public DateOnly? ActivityDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
