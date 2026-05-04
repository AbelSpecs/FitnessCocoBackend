using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Queries.GetExerciseQuery
{
    public class GetExerciseQuery : IRequest<Response<ExerciseDto>>
    {
        public int Id { get; set; }
    }
}
