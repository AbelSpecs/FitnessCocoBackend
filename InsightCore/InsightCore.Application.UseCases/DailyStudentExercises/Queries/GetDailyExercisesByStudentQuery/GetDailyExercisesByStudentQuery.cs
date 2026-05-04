using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Queries.GetDailyExercisesByStudentQuery
{
    public class GetDailyExercisesByStudentQuery : IRequest<Response<IEnumerable<AssignDailyExerciseDto>>>
    {
        public int StudentId { get; set; }
    }
}
