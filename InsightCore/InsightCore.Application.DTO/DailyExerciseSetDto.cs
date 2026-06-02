using System;
using System.Collections.Generic;
using System.Text;

namespace InsightCore.Application.DTO
{
    public class DailyExerciseSetDto
    {
        public int Id { get; set; }
        public int DailyStudentExerciseId { get; set; }
        public int SetNumber { get; set; }
        public int TargetReps { get; set; }
        public decimal TargetWeight { get; set; }
        public string? RestTime { get; set; }
        public int? ActualReps { get; set; }
        public decimal? ActualWeight { get; set; }
        public bool IsAchieved { get; set; }
    }
}
