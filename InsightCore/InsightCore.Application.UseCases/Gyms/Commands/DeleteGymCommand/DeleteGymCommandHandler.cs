using InsightCore.Application.Interface.Persistence;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.DeleteGymCommand
{
    public class DeleteGymCommandHandler : IRequestHandler<DeleteGymCommand, Response<bool>>
    {
        private readonly IGymsRepository _gymsRepository;

        public DeleteGymCommandHandler(IGymsRepository gymsRepository)
        {
            _gymsRepository = gymsRepository;
        }

        public async Task<Response<bool>> Handle(DeleteGymCommand request, CancellationToken cancellationToken)
        {
            var response = new Response<bool>();
            try
            {
                var deleted = await _gymsRepository.DeleteAsync(request.Id);
                response.Data = deleted;
                response.IsSuccess = deleted;
                response.Message = deleted ? "Gym deleted successfully." : "Gym not found or could not be deleted.";
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
