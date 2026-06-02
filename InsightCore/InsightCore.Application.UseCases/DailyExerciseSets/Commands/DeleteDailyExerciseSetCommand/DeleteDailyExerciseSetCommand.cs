using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.DeleteDailyExerciseSetCommand
{
    public class DeleteDailyExerciseSetCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
