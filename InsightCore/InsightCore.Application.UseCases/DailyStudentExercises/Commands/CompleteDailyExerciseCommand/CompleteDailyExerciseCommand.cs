using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.CompleteDailyExerciseCommand
{
    public class CompleteDailyExerciseCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public CompleteExerciseDto Complete { get; set; } = null!;
    }
}
