using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class CoachDetailDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
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
