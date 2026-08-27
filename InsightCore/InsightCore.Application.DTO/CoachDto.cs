using System;

namespace InsightCore.Application.DTO
{
    public class CoachDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string? Certifications { get; set; }
        public bool IsVerified { get; set; }
        public int YearsOfExperience { get; set; }
        public string? ProfilePicture { get; set; }
        public string? BannerPicture { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
