using InsightCore.Application.DTO;
using InsightCore.Transversal.Common;
using MediatR;

namespace InsightCore.Application.UseCases.MuscleGroups.Queries.GetMuscleGroupQuery
{
    public class GetMuscleGroupQuery : IRequest<Response<MuscleGroupDto>>
    {
        public int Id { get; set; }
    }
}
