using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InsightCore.Application.UseCases.MuscleGroups.Commands.DeleteMuscleGroupCommand
{
    public class DeleteMuscleGroupCommandHandler : IRequestHandler<DeleteMuscleGroupCommand, Response<bool>>
    {
        private readonly IMuscleGroupsRepository _repository;

        public DeleteMuscleGroupCommandHandler(IMuscleGroupsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<bool>> Handle(DeleteMuscleGroupCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _repository.DeleteAsync(request.Id);
                response.IsSuccess = deleted;
                response.Data = deleted;
                response.Message = deleted ? "Muscle group deleted." : "Could not delete muscle group (maybe has exercises or not found).";
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
