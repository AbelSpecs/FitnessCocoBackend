using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.UpdateGymCommand
{
    public class UpdateGymCommand : IRequest<Response<GymDto>>
    {
        public int Id { get; set; }
        public GymDto Gym { get; set; } = null!;
    }
}
