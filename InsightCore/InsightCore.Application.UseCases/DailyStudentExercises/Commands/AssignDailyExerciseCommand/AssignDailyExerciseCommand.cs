using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.AssignDailyExerciseCommand
{
    public class AssignDailyExerciseCommand : IRequest<Response<AssignDailyExerciseDto>>
    {
        public AssignDailyExerciseDto Assign { get; set; } = null!;
    }
}
