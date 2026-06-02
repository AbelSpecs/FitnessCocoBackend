using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.MuscleGroups.Commands.CreateMuscleGroupCommand
{
    public class CreateMuscleGroupCommand : IRequest<Response<MuscleGroupDto>>
    {
        public MuscleGroupDto MuscleGroup { get; set; } = null!;
    }
}
