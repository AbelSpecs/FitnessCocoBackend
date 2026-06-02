using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.UpdateExerciseCommand
{
    public class UpdateExerciseCommand : IRequest<Response<ExerciseDto>>
    {
        public int Id { get; set; }
        public ExerciseUpdateDto Exercise { get; set; } = null!;
    }
}
