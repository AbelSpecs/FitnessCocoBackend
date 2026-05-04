using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Exercises.Commands.DeleteExerciseCommand
{
    public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand, Response<bool>>
    {
        private readonly IExercisesRepository _exercisesRepository;

        public DeleteExerciseCommandHandler(IExercisesRepository exercisesRepository)
        {
            _exercisesRepository = exercisesRepository;
        }

        public async Task<Response<bool>> Handle(DeleteExerciseCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _exercisesRepository.DeleteAsync(request.Id);
                response.IsSuccess = deleted;
                response.Data = deleted;
                response.Message = deleted ? "Exercise deleted." : "Exercise not found.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
