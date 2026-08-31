using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class ExerciseCreateDto
    {
        public int? CoachId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int MuscleGroupId { get; set; }
        public string? VideoKey { get; set; }
        public bool IsCustom { get; set; }
    }
}
