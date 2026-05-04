using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.CreateExerciseCommand
{
    public class CreateExerciseCommand : IRequest<Response<ExerciseDto>>
    {
        public ExerciseDto Exercise { get; set; } = null!;
    }
}