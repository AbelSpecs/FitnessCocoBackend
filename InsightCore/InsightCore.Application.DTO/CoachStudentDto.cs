using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class CoachStudentDto
    {
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public DateTime AssignedAt { get; set; }
        public string Status { get; set; } = "Active";
    }

    // DTO para crear la asignación de un nuevo alumno a un coach
    public class AssignStudentDto
    {
        public int CoachId { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; } = "Active";
    }

    // DTO para actualizar el estado del alumno (ej. Pausar o Cancelar la asesoría)
    public class UpdateStudentStatusDto
    {
        public string Status { get; set; } = null!;
    }
}
