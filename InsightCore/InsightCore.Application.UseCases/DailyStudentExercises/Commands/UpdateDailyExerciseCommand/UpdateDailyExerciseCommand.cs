using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.UpdateDailyExerciseCommand
{
    public class UpdateDailyExerciseCommand : IRequest<Response<AssignDailyExerciseDto>>
    {
        public int Id { get; set; }
        public AssignDailyExerciseDto Assign { get; set; } = null!;
    }
}
