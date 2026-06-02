using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.CreateDailyExerciseSetCommand
{
    public class CreateDailyExerciseSetCommand : IRequest<Response<DailyExerciseSetDto>>
    {
        public DailyExerciseSetDto Set { get; set; } = null!;
    }
}
