using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.DeleteExerciseCommand
{
    public class DeleteExerciseCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
