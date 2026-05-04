using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
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

    // DTO específico para cuando creas o actualizas un alumno
    public class CreateOrUpdateStudentDto
    {
        public int UserId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public string? FitnessGoal { get; set; }
        public string? ActivityLevel { get; set; }
        public string? MedicalConditions { get; set; }
        public string? Allergies { get; set; }
        public string? FitnessExperience { get; set; }
        public string? GeneralNotes { get; set; }
    }
}
