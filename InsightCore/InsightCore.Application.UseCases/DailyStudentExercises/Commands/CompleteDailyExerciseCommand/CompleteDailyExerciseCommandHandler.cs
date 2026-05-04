using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.DailyStudentExercises.Commands.CompleteDailyExerciseCommand
{
    public class CompleteDailyExerciseCommandHandler : IRequestHandler<CompleteDailyExerciseCommand, Response<bool>>
    {
        private readonly IDailyStudentExercisesRepository _dailyRepo;

        public CompleteDailyExerciseCommandHandler(IDailyStudentExercisesRepository dailyRepo)
        {
            _dailyRepo = dailyRepo;
        }

        public async Task<Response<bool>> Handle(CompleteDailyExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var existing = await _dailyRepo.GetByIdAsync(request.Id);
                if (existing == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Not found.";
                    response.Data = false;
                    return response;
                }
                existing.IsCompleted = request.Complete.IsCompleted;
                existing.StudentNotes = request.Complete.StudentNotes;
                var updated = await _dailyRepo.UpdateAsync(existing);
                response.IsSuccess = updated;
                response.Data = updated;
                response.Message = updated ? "Completed." : "No changes.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Data = false;
            }
            return response;
        }
    }
}
