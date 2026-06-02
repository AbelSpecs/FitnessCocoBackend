using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.DailyExerciseSets.Commands.DeleteDailyExerciseSetCommand
{
    public class DeleteDailyExerciseSetCommandHandler : IRequestHandler<DeleteDailyExerciseSetCommand, Response<bool>>
    {
        private readonly IDailyExerciseSetsRepository _repository;

        public DeleteDailyExerciseSetCommandHandler(IDailyExerciseSetsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeleteDailyExerciseSetCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _repository.DeleteAsync(request.Id);
                response.IsSuccess = deleted;
                response.Data = deleted;
                response.Message = deleted ? "Set deleted." : "Set not found.";
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
