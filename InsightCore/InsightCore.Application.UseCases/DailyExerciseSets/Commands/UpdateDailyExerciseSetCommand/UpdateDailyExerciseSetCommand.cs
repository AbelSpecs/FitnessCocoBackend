using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.UpdateDailyExerciseSetCommand
{
    public class UpdateDailyExerciseSetCommand : IRequest<Response<DailyExerciseSetDto>>
    {
        public int Id { get; set; }
        public DailyExerciseSetDto Set { get; set; } = null!;
    }
}
