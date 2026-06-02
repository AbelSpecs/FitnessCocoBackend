using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;
using System.Collections.Generic;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleGroupQuery
{
    public class GetExercisesByMuscleGroupQuery : IRequest<Response<IEnumerable<ExerciseDto>>>
    {
        public int MuscleGroupId { get; set; }
    }
}
