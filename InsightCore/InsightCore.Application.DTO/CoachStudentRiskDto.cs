namespace PyrosFit.Application.DTOs
{
    public class CoachStudentRiskDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public int CurrentStreak { get; set; }
        public int DaysInactive { get; set; }
        public int RiskLevel { get; set; }
    }
}
