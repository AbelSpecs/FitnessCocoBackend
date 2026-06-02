using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.MuscleGroups.Commands.DeleteMuscleGroupCommand
{
    public class DeleteMuscleGroupCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
    }
}
