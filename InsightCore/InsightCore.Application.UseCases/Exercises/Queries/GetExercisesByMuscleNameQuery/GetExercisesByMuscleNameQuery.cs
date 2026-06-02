using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExercisesByMuscleNameQuery
{
    public class GetExercisesByMuscleNameQuery : IRequest<Response<IEnumerable<ExerciseDto>>>
    {
        public string Name { get; set; } = null!;
    }
}
