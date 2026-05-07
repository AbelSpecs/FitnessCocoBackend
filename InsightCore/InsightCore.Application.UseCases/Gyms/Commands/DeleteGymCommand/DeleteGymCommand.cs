using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.DeleteGymCommand
{
    public class DeleteGymCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
