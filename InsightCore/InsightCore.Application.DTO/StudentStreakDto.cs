using System;
using System.Text.Json.Serialization;

namespace InsightCore.Application.DTO
{
    public class StudentStreakDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateOnly? LastCompletedDate { get; set; }
        public int FreezeShieldsAvailable { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsCompletedToday { get; set; }
        public int DaysInactive { get; set; }
        public string Status { get; set; } = "Active";
    }
}
