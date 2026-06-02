using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Queries.GetDailyExerciseSetQuery
{
    public class GetDailyExerciseSetQuery : IRequest<Response<DailyExerciseSetDto>>
    {
        public int Id { get; set; }
    }
}
