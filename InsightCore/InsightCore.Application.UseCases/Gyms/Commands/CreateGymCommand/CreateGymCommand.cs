using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.Gyms.Commands.CreateGymCommand
{
    public class CreateGymCommand : IRequest<Response<GymDto>>
    {
        public GymDto Gym { get; set; } = null!;
    }
}
