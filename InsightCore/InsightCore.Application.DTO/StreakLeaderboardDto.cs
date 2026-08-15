using System;

namespace InsightCore.Application.DTO
{
    public class StreakLeaderboardDto
    {
        public int Rank { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateOnly? LastCompletedDate { get; set; }
        public int FreezeShieldsAvailable { get; set; }
    }
}
