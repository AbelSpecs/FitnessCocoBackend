using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByCoachQuery
{
    public class GetExercisesByCoachQuery : IRequest<Response<IEnumerable<ExerciseDto>>>
    {
        public int CoachId { get; set; }
    }
}
