using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class StudentDetailDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
        public int UserId { get; set; }

        // Información Física
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BodyFatPercentage { get; set; }

        // Historial de Salud y Objetivos
        public string? FitnessGoal { get; set; }
        public string? ActivityLevel { get; set; }
        public string? MedicalConditions { get; set; }
        public string? Allergies { get; set; }
        public string? FitnessExperience { get; set; }

        public string? GeneralNotes { get; set; }

    }
}
