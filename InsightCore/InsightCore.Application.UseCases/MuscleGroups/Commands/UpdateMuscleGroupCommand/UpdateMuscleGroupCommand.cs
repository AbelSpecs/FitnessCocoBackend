using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.MuscleGroups.Commands.UpdateMuscleGroupCommand
{
    public class UpdateMuscleGroupCommand : IRequest<Response<MuscleGroupDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
