using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class StudentListByCoachDto
    {
        public List<CoachStudentItemDto> Students { get; set; } = new();

    }

    public class CoachStudentItemDto
    {
        public int StudentId { get; set; }
        public string? Name { get; set; }
        public string? FitnessGoal { get; set; }
    }
}
