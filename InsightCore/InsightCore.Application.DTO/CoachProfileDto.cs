namespace InsightCore.Application.DTO
{
    /// <summary>
    /// Perfil completo del entrenador incluyendo metricas de alumnos, rutinas y valoracion promedio.
    /// </summary>
    public class CoachProfileDto
    {
        // Datos basicos
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Certifications { get; set; }
        public bool IsVerified { get; set; }
        public int YearsOfExperience { get; set; }

        // Imagenes (Base64 o URL)
        public string? ProfilePicture { get; set; }
        public string? BannerPicture { get; set; }

        // Metricas de alumnos
        public int TotalStudents { get; set; }
        public int ActiveStudents { get; set; }
        public int InactiveStudents { get; set; }
        public int TotalRoutinesCreated { get; set; }

        // Valoraciones
        public double AverageRating { get; set; }
        public int TotalRatingsCount { get; set; }
    }
}
