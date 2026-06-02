using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class ExerciseDto
    {
        public int Id { get; set; }
        public int? CoachId { get; set; } // NULL si lo crea un administrador como global
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        // Id de la relación con MuscleGroup (para create/update)
        public int MuscleGroupId { get; set; }
        // Nombre del grupo muscular (para respuestas al frontend)
        public string? MuscleGroup { get; set; }
        public string? VideoUrl { get; set; }
        public bool IsCustom { get; set; }
    }

    // Para que el Coach asigne un ejercicio diario al Alumno
    public class AssignDailyExerciseDto
    {
        public int Id { get; set; }
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime ScheduledDate { get; set; }
        // Replaced scalar set fields by a collection of planned sets
        public IEnumerable<DailyExerciseSetDto>? DailyExerciseSets { get; set; }
        // Convenience fields populated when returning assigned exercises
        public string? ExerciseName { get; set; }
        public string? MuscleGroupName { get; set; }
        public string? CoachNotes { get; set; }
    }

    // Para que el alumno marque como completado su ejercicio y deje feedback
    public class CompleteExerciseDto
    {
        public bool IsCompleted { get; set; }
        public string? StudentNotes { get; set; }
    }
}
