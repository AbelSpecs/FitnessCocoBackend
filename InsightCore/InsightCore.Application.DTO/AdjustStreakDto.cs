namespace InsightCore.Application.DTO
{
    public class AdjustStreakDto
    {
        public int? CurrentStreak { get; set; }
        public int? LongestStreak { get; set; }
        public int? FreezeShields { get; set; }
        public string? Reason { get; set; }
    }
}
